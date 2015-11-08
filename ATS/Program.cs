using System.Collections.Generic;
using System.Linq;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;
using ATS.TestAts;

namespace ATS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ports = new List<IPort>() { new TestPort(), new TestPort() };
            var ats = new Ats(ports,new List<ITerminal>());

            ats.Add(new TestTerminal(new PhoneNumber("000-00-0")));
            ats.Add(new TestTerminal(new PhoneNumber("111-11-1")));

            foreach (var v in ats.TerminalCollection)
            {
                v.Plug();
                v.Unplug();
            }

            /*ats.TerminalCollection.ElementAt(0).Call(new PhoneNumber("111-11-1"));

            ats.WaitActionTerminals.ElementAt(0).Answer();*/

            ats.PortCollection.ElementAt(0).State = PortState.Unpluged;
            ats.PortCollection.ElementAt(0).State = PortState.Free;


        }
    }
}