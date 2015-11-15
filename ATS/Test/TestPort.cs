using System;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Test
{
    public class TestPort : Port
    {
        public override void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.Plugging += (port, args) => { State = PortState.Free; };
            terminal.UnPlugging += (port, args) => { State = PortState.Unpluged; };
            StateChanged += (sender, state) => { Console.WriteLine($"Port detect change his State to {State}"); };
        }
    }
}