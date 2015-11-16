using System;
using System.Collections.Generic;
using System.Linq;
using ATS.Helpers;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Test
{
    public class Ats : Station
    {
        public Ats(ICollection<IPort> ports, ICollection<ITerminal> terminals) : base(ports, terminals)
        {
        }

        protected override void InnerConnectionHandler(object sender, CallInfo callInfo)
        {
            var targetPort = GetPortByPhoneNumber(callInfo.Target);
            if (targetPort.State == PortState.Unpluged || targetPort.State == PortState.Call ||
                callInfo.Source == callInfo.Target)
            {
                SetTerminalStateTo(callInfo.Source, TerminalState.Free);

                callInfo.TimeBegin = TimeHelper.Now;
                callInfo.Duration = TimeSpan.Zero;
                OnCallInfoPrepared(this, callInfo);
            }
            else
            {
                SetPortsStateTo(callInfo.Source, callInfo.Target, PortState.Call);
                SetTerminalStateTo(callInfo.Source, TerminalState.OutGoingCall);
                SetTerminalStateTo(callInfo.Target, TerminalState.IncomingCall);
                var targetTerminal = _terminalCollection.FirstOrDefault(x => x.Number == callInfo.Target);

                targetTerminal?.GetReqest(callInfo.Source);
                _callInfoCollection.Add(callInfo);
                _waitActionTerminals.Add(targetTerminal);
            }
        }

        private void SetTerminalStateTo(PhoneNumber source, TerminalState state)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(source) as TestTerminal;

            if (sourceTerminal != null) sourceTerminal.State = state;
        }

        public override void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.OutgoingCall += InnerConnectionHandler;
            terminal.Responce += ResponseConnectionHandler;
        }

        public override void RegisterEventHandlersForPort(IPort port)
        {
            port.StateChanged +=
                (sender, state) => { Console.WriteLine("Station detected the port changed its State to {0}", state); };
            port.StateChanging += (sender, state) =>
            {
                var a = PortsMapping.FirstOrDefault(x => x.Value == sender).Key;
                var d = GetTerminalByPhoneNumber(a);

                if (_activeCallMapping.ContainsKey(d) || _activeCallMapping.Values.Contains(d))
                {
                    d.Drop();
                }
                else
                {
                    d.Reject();
                }
            };
        }

        protected override void ResponseConnectionHandler(object sender, Response responce)
        {
            if (responce.State == ResponseState.Drop || responce.State == ResponseState.Reject)
            {
                var callinfo = GetCallInfo(responce.Source);
                SetTerminalsStateTo(callinfo.Source, callinfo.Target, TerminalState.Free);
            }
            base.ResponseConnectionHandler(sender, responce);
        }

        private void SetTerminalsStateTo(PhoneNumber source, PhoneNumber target, TerminalState state)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(source) as TestTerminal;
            var targetTerminal = GetTerminalByPhoneNumber(target) as TestTerminal;
            if (sourceTerminal != null) sourceTerminal.State = state;
            if (targetTerminal != null) targetTerminal.State = state;
        }
    }
}