using ATS.BillingSystemModel.AbstractClass;
using ATS.Station_Model.AbstractClasses;
using ATS.TestAts;

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
        }

    }
}