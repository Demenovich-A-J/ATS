using System;
using System.Collections.Generic;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.BillingSystemModel.Intarfaces
{
    public interface IBillingSystem
    {
        ICollection<ITariffPlan> TariffPlans { get; }
        ITerminal GetContract(IUser user, ITariffPlan tariffPlan);
        void CallInfoHandler(object sender, CallInfo callInfo);
        event EventHandler<IUser> Pay;
        event EventHandler<ITerminal> ToSignСontract;
        void PayForPhoneNubmer(PhoneNumber phone);
    }
}