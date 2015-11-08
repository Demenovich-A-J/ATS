using System;
using System.Collections.Generic;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;

namespace ATS.TestAts
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
    }
}