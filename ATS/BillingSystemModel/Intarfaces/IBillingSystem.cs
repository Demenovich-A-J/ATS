using System.Collections.Generic;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.BillingSystemModel.Intarfaces
{
    public interface IBillingSystem
    {
        ICollection<ITariffPlan> TariffPlans { get; }
        IDictionary<ITerminal,IUser> TerminalsUserMapp { get; }
        ITerminal GetContract(IUser user);
        void CallInfoHandler(object sender,CallInfo callInfo);
    }
}