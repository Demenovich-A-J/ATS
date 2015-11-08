using System;
using System.Collections.Generic;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;

namespace ATS.Station_Model
{
    public class Ats : Station
    {
        public Ats(List<IPort> ports, List<ITerminal> terminals) : base(ports, terminals)
        {

        }


        public override void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.OutgoingCall += InnerConnectionHandler;
        }

        public override void RegisterEventHandlersForPort(IPort port)
        {
            port.StateChanged +=
                (sender, state) => { Console.WriteLine("Station detected the port changed its State to {0}", state); };
        }
    }
}