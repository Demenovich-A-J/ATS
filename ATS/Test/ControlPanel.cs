using System;
using System.Collections.Generic;
using System.Linq;
using ATS.BillingSystemModel.AbstractClass;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Helpers;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;
using ATS.User_Model;

namespace ATS.Test
{
    public class ControlPanel
    {
        private readonly Station _ats;
        private readonly BillingSystem _billingSystem;
        private readonly ICollection<IUser> _disconectedUserCollection;

        public ControlPanel(Station ats, BillingSystem bilingSystem)
        {
            _ats = ats;
            _billingSystem = bilingSystem;
            _disconectedUserCollection = new List<IUser>();
            _ats.CallInfoPrepared += _billingSystem.CallInfoHandler;
            TimeHelper.NewDateTime += DateChangeHandler;
            _billingSystem.Pay += PayHandler;
            _billingSystem.ToSignСontract += GetContracTerminal;

        }

        public void GetContracTerminal(object sender,ITerminal terminal)
        {
            if (_ats.Add(terminal)) return;
            _ats.AddPort(new TestPort());
            _ats.Add(terminal);
        }

        private void DateChangeHandler(object sender, DateTime time)
        {
            foreach (
                var info in
                    _billingSystem.UserPayDateTime.Where(info => info.Value.AddMonths(2) <= time)
                        .Where(info => !_disconectedUserCollection.Contains(info.Key)))
            {
                info.Key.Drop();
                info.Key.Phone.Unplug();
                info.Key.Phone.ClearEvents();
                _ats.PortsMapping[info.Key.Phone.Number].ClearEvents();
                _disconectedUserCollection.Add(info.Key);

                Console.WriteLine("|=====================================================|");
                Console.WriteLine(
                    $"Abonent : {info.Key.Phone.Number.Number}; Was disconect from station. Because of non-payment;");
                Console.WriteLine("|=====================================================|");
            }
        }

        private void PayHandler(object sender, IUser user)
        {
            if(!_disconectedUserCollection.Contains(user)) return;

            var sourcePort = _ats.PortsMapping[user.Phone.Number];

            _billingSystem.UserPayDateTime[user] = TimeHelper.Now;
            _ats.PortsMapping[user.Phone.Number].RegisterEventHandlersForTerminal(user.Phone);
            _ats.RegisterEventHandlersForTerminal(user.Phone);
            _ats.RegisterEventHandlersForPort(sourcePort);
            user.Phone.RegisterEventHandlersForPort(sourcePort);
            user.Plug();
            _disconectedUserCollection.Remove(user);

            Console.WriteLine($"Abonent : {user.Phone.Number.Number}; Pay for his phone. And can resume calls.");
        }
    }
}