namespace ATS.BillingSystemModel.Intarfaces
{
    public interface ITariffPlan
    {
        double CostOneMinute { get; }
        double FreeMinutes { get; set; }
    }
}