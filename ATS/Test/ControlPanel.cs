using ATS.BilingSystemModel.AbstractClass;
using ATS.Station_Model.AbstractClasses;
using ATS.TestAts;

namespace ATS.Test
{
    public class ControlPanel
    {

        private readonly Station _ats;
        private readonly BilingSystem _bilingSystem;

        public ControlPanel(Station ats, BilingSystem bilingSystem)
        {
            _ats = ats;
            _bilingSystem = bilingSystem;
        }

    }
}