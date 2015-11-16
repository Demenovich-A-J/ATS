using System;
using ATS.BillingSystemModel.Intarfaces;

namespace ATS.BillingSystemModel.TarifPlans
{
    public class EasyTalk : ITariffPlan
    {
        public static double CostOneMinute => 25.1;

        public double FreeMinutes { get; private set; } = 100;

        public double CalculateCallCost(TimeSpan duration)
        {
            if (FreeMinutes == 0) return CostOneMinute*Math.Abs(duration.TotalMinutes);
            if (FreeMinutes - Math.Abs(duration.TotalMinutes) < 0)
            {
                FreeMinutes = 0;

                return CostOneMinute*(Math.Abs(duration.TotalMinutes) - FreeMinutes);
            }

            FreeMinutes -= Math.Abs(duration.TotalMinutes);

            return 0;
        }
    }
}