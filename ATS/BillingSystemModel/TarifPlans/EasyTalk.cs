using ATS.BillingSystemModel.Intarfaces;

namespace ATS.BillingSystemModel.TarifPlans
{
    public class EasyTalk : ITariffPlan
    {
        public EasyTalk(double costOneCall, int freeMinutes)
        {
            CostOneMinute = costOneCall;
            FreeMinutes = freeMinutes;
        }

        public double CostOneMinute { get; }
        public double FreeMinutes { get; set; }
    }
}