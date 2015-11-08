using System.Collections.Generic;
using System.Linq;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Station_Model.AbstractClasses
{
    public abstract class Station : IStation
    {
        private List<IPort> _ports;
        private readonly List<ITerminal> _terminals;
        private readonly List<CallInfo> _outCallsCollection;
        private readonly List<ITerminal> _waitActionTerminals; 
        private readonly Dictionary<PhoneNumber, IPort> _phonePortsMap;
        protected Station(List<IPort> ports, List<ITerminal> terminals)
        {
            _ports = ports;
            _terminals = terminals;
            _phonePortsMap = new Dictionary<PhoneNumber, IPort>();
            _outCallsCollection = new List<CallInfo>();
            _waitActionTerminals = new List<ITerminal>();
        }

        protected virtual void InnerConnectionHandler(object sender, CallInfo info)
        {
            var targetPort = GetPortByPhoneNumber(info.Target);
            if (targetPort.State == PortState.Unpluged || targetPort.State == PortState.Call) return;

            var sourcePort = GetPortByPhoneNumber(info.Source);
            _outCallsCollection.Add(info);

            _waitActionTerminals.Add(_terminals.FirstOrDefault(x => x.Number == info.Target));
        }

        protected virtual void ResponseConnectionHandler(object sender, PhoneNumber target)
        {

            var callInfo = GetCallInfo(target);
            
            InterruptActiveCall(callInfo);
        }

        private void InterruptActiveCall(CallInfo info)
        {
            _outCallsCollection.Remove(info);
            _waitActionTerminals.Remove(GetTerminalByPhoneNumber(info.Source));

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
            return _terminals.FirstOrDefault(x => x.Number == number);
        }

        protected virtual CallInfo GetCallInfo(PhoneNumber target)
        {
            return _outCallsCollection.FirstOrDefault(x => x.Target == target);
        }

        protected virtual IPort GetPortByPhoneNumber(PhoneNumber number)
        {
            return _phonePortsMap[number];
        }

        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        public abstract void RegisterEventHandlersForPort(IPort port);
    }
}