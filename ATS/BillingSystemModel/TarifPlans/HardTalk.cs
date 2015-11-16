using System;
using ATS.BillingSystemModel.Intarfaces;

namespace ATS.BillingSystemModel.TarifPlans
{
    public class HardTalk : ITariffPlan
    {
        public static double CostOneSecond => 25.1;

        public double FreeMinutes { get; private set; }

        public double CalculateCallCost(TimeSpan duration)
        {
            if (FreeMinutes == 0) return CostOneSecond*Math.Abs(duration.TotalSeconds);
            if (FreeMinutes - Math.Abs(duration.TotalSeconds) < 0)
            {
                FreeMinutes = 0;

                return CostOneSecond*(Math.Abs(duration.TotalSeconds) - FreeMinutes);
            }

            FreeMinutes -= Math.Abs(duration.TotalSeconds);

            return 0;
        }
    }
}