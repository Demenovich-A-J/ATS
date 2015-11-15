using System;
using System.Collections.Generic;
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


        public override void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.OutgoingCall += InnerConnectionHandler;
            terminal.Responce += ResponseConnectionHandler;
        }

        public override void RegisterEventHandlersForPort(IPort port)
        {
            port.StateChanged +=
                (sender, state) => { Console.WriteLine("Station detected the port changed its State to {0}", state); };
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

        private void SetTerminalsStateTo(PhoneNumber source, PhoneNumber target,TerminalState state)
        {
            var sourceTerminal = GetTerminalByPhoneNumber(source) as TestTerminal;
            var targetTerminal = GetTerminalByPhoneNumber(target) as TestTerminal;
            if (sourceTerminal != null) sourceTerminal.State = state;
            if (targetTerminal != null) targetTerminal.State = state;
        }
    }
}