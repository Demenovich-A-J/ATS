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
        private readonly ICollection<CallInfo> _outCallsCollection;
        private readonly ICollection<ITerminal> _waitActionTerminals; 
        private readonly IDictionary<PhoneNumber, IPort> _portsMapping;
        protected Station(ICollection<IPort> ports, ICollection<ITerminal> terminals)
        {
            _portCollection = ports;
            _terminalCollection = terminals;
            _portsMapping = new Dictionary<PhoneNumber, IPort>();
            _outCallsCollection = new List<CallInfo>();
            _waitActionTerminals = new List<ITerminal>();
        }

        public ICollection<ITerminal> TerminalCollection => _terminalCollection;

        public ICollection<ITerminal> WaitActionTerminals => _waitActionTerminals;

        public ICollection<IPort> PortCollection => _portCollection;

        protected virtual void InnerConnectionHandler(object sender, CallInfo info)
        {
            var targetPort = GetPortByPhoneNumber(info.Target);
            if (targetPort.State == PortState.Unpluged || targetPort.State == PortState.Call) return;

            var sourcePort = GetPortByPhoneNumber(info.Source);

            sourcePort.State = PortState.Call;
            targetPort.State = PortState.Call;


            var targetTerminal = TerminalCollection.FirstOrDefault(x => x.Number == info.Target);
            targetTerminal?.GetReqest(info.Source);
            _outCallsCollection.Add(info);
            WaitActionTerminals.Add(targetTerminal);
        }

        protected virtual void ResponseConnectionHandler(object sender, Responce responce)
        {
            var callInfo = GetCallInfo(responce.Source);

            switch (responce.State)
            {
                case ResponseState.Accept:
                    MakeCallActive(callInfo);
                    InterruptActiveCall(callInfo);
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
            WaitActionTerminals.Remove(GetTerminalByPhoneNumber(info.Source));
            info.TimeBegin = DateTime.Now;
            Thread.Sleep(new Random().Next(500, 1000));
            info.Duration = info.TimeBegin - DateTime.Now;
        }

        private void InterruptCall(CallInfo info)
        {
            _outCallsCollection.Remove(info);
            WaitActionTerminals.Remove(GetTerminalByPhoneNumber(info.Source));
            ResetPortsAfterCall(info.Source, info.Target);
        }
        private void InterruptActiveCall(CallInfo info)
        {
            _outCallsCollection.Remove(info);
            ResetPortsAfterCall(info.Source,info.Target);
        }

        private void ResetPortsAfterCall(PhoneNumber source, PhoneNumber target)
        {
            var targetPort = GetPortByPhoneNumber(target);
            var sourcePort = GetPortByPhoneNumber(source);

            if (targetPort != null)
            {
                targetPort.State = PortState.Free;
            }
            if (sourcePort != null)
            {
                sourcePort.State = PortState.Free;
            }
        }

        private ITerminal GetTerminalByPhoneNumber(PhoneNumber number)
        {
            return TerminalCollection.FirstOrDefault(x => x.Number == number);
        }

        protected virtual CallInfo GetCallInfo(PhoneNumber target)
        {
            return _outCallsCollection.FirstOrDefault(x => x.Target == target);
        }

        protected virtual IPort GetPortByPhoneNumber(PhoneNumber number)
        {
            return _portsMapping[number];
        }

        public void Add(ITerminal terminal)
        {
            var freePort = PortCollection.Except(_portsMapping.Values).FirstOrDefault();
            if (freePort != null)
            {
                TerminalCollection.Add(terminal);

                MapTerminalToPort(terminal, freePort);

                RegisterEventHandlersForTerminal(terminal);
                RegisterEventHandlersForPort(freePort);
            }
        }

        private void MapTerminalToPort(ITerminal terminal, IPort port)
        {
            _portsMapping.Add(terminal.Number, port);
            port.RegisterEventHandlersForTerminal(terminal);
            terminal.RegisterEventHandlersForPort(port);
        }

        protected void UnMapTerminalFromPort(ITerminal terminal, IPort port)
        {
            _portsMapping.Remove(terminal.Number);
        }

        public event EventHandler<CallInfo> CallInfoPrepared;
        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        public abstract void RegisterEventHandlersForPort(IPort port);
    }
}