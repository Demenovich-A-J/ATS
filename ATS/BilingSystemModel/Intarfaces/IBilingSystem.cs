using System.Collections.Generic;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.BilingSystemModel.Intarfaces
{
    public interface IBilingSystem
    {
        ICollection<ITariffPlan> TariffPlans { get; }
        IDictionary<ITerminal,IUser> TerminalsUserMapp { get; }
        ITerminal GetContract(IUser user);
        void CallInfoHandler(object sender,CallInfo callInfo);
    }
}