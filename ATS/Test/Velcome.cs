using System;
using System.Collections.Generic;
using System.Data;
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
    }
}