using ATS.BillingSystemModel.AbstractClass;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.Test
{
    public class ControlPanel
    {

        private readonly Station _ats;
        private readonly BillingSystem _bilingSystem;

        public ControlPanel(Station ats, BillingSystem bilingSystem)
        {
            _ats = ats;
            _bilingSystem = bilingSystem;
            _ats.CallInfoPrepared += _bilingSystem.CallInfoHandler;
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

    }
}