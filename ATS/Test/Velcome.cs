using System.Collections.Generic;
using ATS.BillingSystemModel.AbstractClass;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.Test
{
    public class Velcome : BillingSystem
    {
        public Velcome(ICollection<ITariffPlan> tariffPlans, IDictionary<ITerminal, IUser> terminalsUserMapp) : base(tariffPlans, terminalsUserMapp)
        {
        }
    }
}