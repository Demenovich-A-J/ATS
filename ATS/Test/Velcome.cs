using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ATS.BillingSystemModel.AbstractClass;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.Test
{
    public class Velcome : BillingSystem
    {
        public Velcome(ICollection<ITariffPlan> tariffPlans) : base(tariffPlans)
        {
            
        }

        public IEnumerable<CallInfo> GetStatistic(Func<CallInfo,bool> predicat ,IUser user)
        {
            return UserCallinfoDictionary.FirstOrDefault(x => x.Key == user).Value.Where(predicat); ;
        }

    }
}