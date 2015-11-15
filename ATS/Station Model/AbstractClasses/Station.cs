using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ATS.Helpers;
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

        public IDictionary<PhoneNumber, IPort> PortsMapping => _portsMapping;

        protected virtual void InnerConnectionHandler(object sender, CallInfo callInfo)
        {
            var targetPort = GetPortByPhoneNumber(callInfo.Target);
            if (targetPort.State == PortState.Unpluged || targetPort.State == PortState.Call || callInfo.Source == callInfo.Target)
            {
                callInfo.TimeBegin = TimeHelper.Now;
                callInfo.Duration = TimeSpan.Zero;
                OnCallInfoPrepared(this, callInfo);
            }
            else
            {
                SetPortsStateTo(callInfo.Source, callInfo.Target, PortState.Call);

                var targetTerminal = _terminalCollection.FirstOrDefault(x => x.Number == callInfo.Target);

                targetTerminal?.GetReqest(callInfo.Source);
                _callInfoCollection.Add(callInfo);
                _waitActionTerminals.Add(targetTerminal);
            }
        }


        protected virtual void ResponseConnectionHandler(object sender, Response responce)
        {
            var callInfo = GetCallInfo(responce.Source);

            switch (responce.State)
            {
                case ResponseState.Accept:
                    MakeCallActive(callInfo);
                    break;
                case ResponseState.Drop:
                    InterruptActiveCall(callInfo);
                    OnCallInfoPrepared(this, callInfo);
                    break;
                case ResponseState.Reject:
                    InterruptCall(callInfo);
                    OnCallInfoPrepared(this, callInfo);
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

            info.TimeBegin = TimeHelper.Now;

        }

        private void InterruptCall(CallInfo info)
        {
            _callInfoCollection.Remove(info);
            SetPortsStateTo(info.Source, info.Target, PortState.Free);
            info.TimeBegin = TimeHelper.Now;
        }
        private void InterruptActiveCall(CallInfo info)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(info.Source);
            var targetTerminal = GetTerminalByPhoneNumber(info.Target);

            info.Duration = TimeHelper.Duration();

            _waitActionTerminals.Remove(targetTerminal);
           
            _activeCallMapping.Remove(new KeyValuePair<ITerminal, ITerminal>(sourceTerminal, targetTerminal));

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

        protected ITerminal GetTerminalByPhoneNumber(PhoneNumber number)
        {
            return _terminalCollection.FirstOrDefault(x => x.Number == number);
        }

        protected virtual CallInfo GetCallInfo(PhoneNumber target)
        {
            return _callInfoCollection.FirstOrDefault(x => x.Target == target);
        }

        protected virtual IPort GetPortByPhoneNumber(PhoneNumber number)
        {
            return PortsMapping[number];
        }

        public void AddPort(IPort port)
        {
            _portCollection.Add(port);
        }
        public bool Add(ITerminal terminal)
        {
            var freePort = _portCollection.Except(PortsMapping.Values).FirstOrDefault();
            if (freePort == null) return false;
            _terminalCollection.Add(terminal);

            MapTerminalToPort(terminal, freePort);

            RegisterEventHandlersForTerminal(terminal);
            RegisterEventHandlersForPort(freePort);

            return true;
        }

        private void MapTerminalToPort(ITerminal terminal, IPort port)
        {
            PortsMapping.Add(terminal.Number, port);
            port.RegisterEventHandlersForTerminal(terminal);
            terminal.RegisterEventHandlersForPort(port);
        }

        private void UnMapTerminalFromPort(ITerminal terminal, IPort port)
        {
            PortsMapping.Remove(terminal.Number);
        }

        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        public abstract void RegisterEventHandlersForPort(IPort port);

        public event EventHandler<CallInfo> CallInfoPrepared;

        protected virtual void OnCallInfoPrepared(object sender,CallInfo callInfo)
        {
            CallInfoPrepared?.Invoke(this, callInfo);
        }

        public void ClearEvents()
        {
            CallInfoPrepared = null;
        }
    }
}