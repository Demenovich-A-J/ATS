using System.Collections.Generic;
using System.Linq;
using ATS.BillingSystemModel.Intarfaces;
using ATS.BillingSystemModel.TarifPlans;
using ATS.Station_Model.Intarfaces;
using ATS.Test;
using ATS.TestAts;
using ATS.User_Model;

namespace ATS
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var ats = new Ats(new List<IPort>(), new List<ITerminal>());
            var velcome = new Velcome(new List<ITariffPlan>());

            var panel = new ControlPanel(ats,velcome);

            var user1 = new User();
            var user2 = new User();


            user1.Phone = panel.GetContracTerminal(user1, new EasyTalk(25.1,0));
            user2.Phone = panel.GetContracTerminal(user2, new EasyTalk(25.1, 0));

            user1.Plug();
            user2.Plug();

            user1.Call(user2.Phone.Number);
            user2.Answer();
            user2.Drop();
        }
    }
}