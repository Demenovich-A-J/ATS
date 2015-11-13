using System;
using System.Collections.Generic;
using System.Linq;
using ATS.BilingSystemModel.Intarfaces;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.User_Model;

namespace ATS.BillingSystemModel.AbstractClass
{
    public abstract class BillingSystem : IBillingSystem
    {
        protected readonly IDictionary<IUser, ICollection<CallInfo>> _userCallinfoDictionary;
        protected readonly IDictionary<IUser, ITariffPlan> _userTariffPlansMapp; 

        protected BillingSystem(ICollection<ITariffPlan> tariffPlans, IDictionary<ITerminal, IUser> terminalsUserMapp)
        {
            TariffPlans = tariffPlans;
            TerminalsUserMapp = terminalsUserMapp;
            _userCallinfoDictionary = new Dictionary<IUser, ICollection<CallInfo>>();
            _userTariffPlansMapp = new Dictionary<IUser, ITariffPlan>();
        }

        public ICollection<ITariffPlan> TariffPlans { get; }
        public IDictionary<ITerminal, IUser> TerminalsUserMapp { get; }

        public ITerminal GetContract(IUser user)
        {
            throw new NotImplementedException();
        }

        public void CallInfoHandler(object sender, CallInfo callInfo)
        {
            var sourceUserMap = GetUserTerminalMapPair(callInfo.Source);
            var targetUserMap = GetUserTerminalMapPair(callInfo.Target);
            var targetCallInfo = new CallInfo(callInfo.Target, callInfo.Source,CallInfoState.IncomingCall)
            {
                TimeBegin = callInfo.TimeBegin,
                Duration = callInfo.Duration,
                Cost = 0
            };

            callInfo.Cost = CalculateCallCost(callInfo.Duration, GeTariffPlan(sourceUserMap.Value));

            _userCallinfoDictionary[sourceUserMap.Value].Add(callInfo);
            _userCallinfoDictionary[targetUserMap.Value].Add(targetCallInfo);
        }

        protected virtual double CalculateCallCost(TimeSpan duration,ITariffPlan userTariffPlan)
        {
            if (userTariffPlan.FreeMinutes != 0)
            {
                if ((userTariffPlan.FreeMinutes - duration.TotalMinutes) < 0)
                {
                    userTariffPlan.FreeMinutes = 0;

                    return userTariffPlan.CostOneMinute*
                                    (duration.TotalMinutes - userTariffPlan.FreeMinutes);
                }

                userTariffPlan.FreeMinutes -= duration.TotalMinutes;

                return 0;
            }

            return userTariffPlan.CostOneMinute* duration.TotalMinutes;
        }

        protected KeyValuePair<ITerminal,IUser> GetUserTerminalMapPair(PhoneNumber number)
        {
            return TerminalsUserMapp.FirstOrDefault(x => x.Key.Number == number);
        }

        protected ITariffPlan GeTariffPlan(IUser user)
        {
            return _userTariffPlansMapp.FirstOrDefault(x => x.Key == user).Value;
        }
    }
}