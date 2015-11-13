using System.Collections.Generic;
using ATS.BilingSystemModel.AbstractClass;
using ATS.BilingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.Test
{
    public class Velcome : BilingSystem
    {
        public Velcome(ICollection<ITariffPlan> tariffPlans, IDictionary<ITerminal, IUser> terminalsUserMapp) : base(tariffPlans, terminalsUserMapp)
        {
        }
    }
}