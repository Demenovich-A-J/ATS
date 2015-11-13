using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Station_Model.AbstractClasses
{
    public abstract class Station : IStation
    {
        private readonly ICollection<IPort> _portCollection;
        private readonly ICollection<ITerminal> _terminalCollection;
        private readonly ICollection<CallInfo> _callInfoCollection;
        private readonly ICollection<ITerminal> _waitActionTerminals;
        private readonly IDictionary<ITerminal, ITerminal> _activeCallMapping;
        private readonly IDictionary<PhoneNumber, IPort> _portsMapping;
        protected Station(ICollection<IPort> ports, ICollection<ITerminal> terminals)
        {
            _portCollection = ports;
            _terminalCollection = terminals;
            _portsMapping = new Dictionary<PhoneNumber, IPort>();
            _callInfoCollection = new List<CallInfo>();
            _waitActionTerminals = new List<ITerminal>();
            _activeCallMapping = new Dictionary<ITerminal, ITerminal>();
        }

        public ICollection<ITerminal> TerminalCollection => _terminalCollection;

        public ICollection<IPort> PortCollection => _portCollection;

        protected virtual void InnerConnectionHandler(object sender, CallInfo info)
        {
            var targetPort = GetPortByPhoneNumber(info.Target);
            if (targetPort.State == PortState.Unpluged || targetPort.State == PortState.Call) return;

            SetPortsStateTo(info.Source,info.Target,PortState.Call);

            var targetTerminal = TerminalCollection.FirstOrDefault(x => x.Number == info.Target);

            targetTerminal?.GetReqest(info.Source);
            _callInfoCollection.Add(info);
            _waitActionTerminals.Add(targetTerminal);
        }


        protected virtual void ResponseConnectionHandler(object sender, Responce responce)
        {
            var callInfo = GetCallInfo(responce.Source);

            switch (responce.State)
            {
                case ResponseState.Accept:
                    MakeCallActive(callInfo);
                    break;
                case ResponseState.Drop:
                    InterruptCall(callInfo);
                    break;
                default:
                    Console.WriteLine($"Invalid Responce from {responce.Source} state = {responce.State} ");
                    break;
            }
        }

        private void MakeCallActive(CallInfo info)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(info.Source);
            var targetTerminal = GetTerminalByPhoneNumber(info.Target);

            _waitActionTerminals.Remove(sourceTerminal);

            _activeCallMapping.Add(sourceTerminal, targetTerminal);

            info.TimeBegin = DateTime.Now;

        }

        private void InterruptCall(CallInfo info)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(info.Source);
            var targetTerminal = GetTerminalByPhoneNumber(info.Target);

            Thread.Sleep(new Random().Next(5000, 10000));
            info.Duration = info.TimeBegin - DateTime.Now;

            if (_waitActionTerminals.Contains(sourceTerminal))
            {
                _waitActionTerminals.Remove(targetTerminal);
            }
            if (_activeCallMapping.ContainsKey(sourceTerminal))
            {
                _activeCallMapping.Remove(new KeyValuePair<ITerminal, ITerminal>(sourceTerminal, targetTerminal));
            }

            _callInfoCollection.Remove(info);
            SetPortsStateTo(info.Source, info.Target,PortState.Free);
        }

        private void SetPortsStateTo(PhoneNumber source, PhoneNumber target, PortState state)
        {
            var targetPort = GetPortByPhoneNumber(target);
            var sourcePort = GetPortByPhoneNumber(source);

            if (targetPort != null)
            {
                targetPort.State = state;
            }
            if (sourcePort != null)
            {
                sourcePort.State = state;
            }
        }

        private ITerminal GetTerminalByPhoneNumber(PhoneNumber number)
        {
            return TerminalCollection.FirstOrDefault(x => x.Number == number);
        }

        protected virtual CallInfo GetCallInfo(PhoneNumber target)
        {
            return _callInfoCollection.FirstOrDefault(x => x.Target == target);
        }

        protected virtual IPort GetPortByPhoneNumber(PhoneNumber number)
        {
            return _portsMapping[number];
        }

        public void Add(ITerminal terminal)
        {
            var freePort = PortCollection.Except(_portsMapping.Values).FirstOrDefault();
            if (freePort == null) return;
            TerminalCollection.Add(terminal);

            MapTerminalToPort(terminal, freePort);

            RegisterEventHandlersForTerminal(terminal);
            RegisterEventHandlersForPort(freePort);
        }

        private void MapTerminalToPort(ITerminal terminal, IPort port)
        {
            _portsMapping.Add(terminal.Number, port);
            port.RegisterEventHandlersForTerminal(terminal);
            terminal.RegisterEventHandlersForPort(port);
        }

        private void UnMapTerminalFromPort(ITerminal terminal, IPort port)
        {
            _portsMapping.Remove(terminal.Number);
        }

        public event EventHandler<CallInfo> CallInfoPrepared;
        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        public abstract void RegisterEventHandlersForPort(IPort port);
    }
}