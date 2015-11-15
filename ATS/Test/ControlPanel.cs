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
        private readonly BillingSystem _bilingSystem;
        private readonly ICollection<IUser> _disconectedUserCollection; 
        public ControlPanel(Station ats, BillingSystem bilingSystem)
        {
            _ats = ats;
            _bilingSystem = bilingSystem;
            _disconectedUserCollection = new List<IUser>();
            _ats.CallInfoPrepared += _bilingSystem.CallInfoHandler;
            TimeHelper.NewDateTime += DateChangeHandler;
        }

        public ITerminal GetContracTerminal(IUser user,ITariffPlan tariffPlan)
        {
            var terminal = _bilingSystem.GetContract(user, tariffPlan);

            if (_ats.Add(terminal))
            {
                return terminal;
            }

            _ats.AddPort(new TestPort());
            _ats.Add(terminal);

            return terminal;
        }

        private void DateChangeHandler(object sender, DateTime time)
        {
            foreach (var info in _bilingSystem.UserDateTimesMapp.Where(info => info.Value.AddMonths(2) <= time).Where(info => !_disconectedUserCollection.Contains(info.Key)))
            {
                info.Key.Drop();
                info.Key.Phone.ClearEvents();

                Console.WriteLine("|=====================================================|");
                _ats.PortsMapping[info.Key.Phone.Number].State = PortState.Unpluged;
                _disconectedUserCollection.Add(info.Key);
                Console.WriteLine($"Abonent : {info.Key.Phone.Number.Number}; Was disconect from station. Because of non-payment;");
                Console.WriteLine("|=====================================================|");
            }
        }
    }
}