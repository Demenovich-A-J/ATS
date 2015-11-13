using System.Collections.Generic;
using System.Linq;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;
using ATS.Test;
using ATS.TestAts;
using ATS.User_Model;

namespace ATS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ports = new List<IPort>() { new TestPort(), new TestPort() };
            var ats = new Ats(ports,new List<ITerminal>());
            var velcome = new Velcome(new List<ITariffPlan>(),new Dictionary<ITerminal, IUser>());

            ats.CallInfoPrepared += velcome.CallInfoHandler;

            ats.Add(new TestTerminal(new PhoneNumber("000-00-0")));
            ats.Add(new TestTerminal(new PhoneNumber("111-11-1")));

            foreach (var v in ats.TerminalCollection)
            {
                v.Plug();
            }

            ats.TerminalCollection.ElementAt(0).Call(new PhoneNumber("111-11-1"));

            ats.TerminalCollection.ElementAt(1).Answer();

            ats.TerminalCollection.ElementAt(1).Drop();


            //ats.PortCollection.ElementAt(0).State = PortState.Unpluged;
            //ats.PortCollection.ElementAt(0).State = PortState.Free;
        }
    }
}