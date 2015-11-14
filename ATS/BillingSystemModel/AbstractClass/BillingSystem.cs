using System;
using System.Collections.Generic;
using System.Linq;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.TestAts;
using ATS.User_Model;

namespace ATS.BillingSystemModel.AbstractClass
{
    public abstract class BillingSystem : IBillingSystem
    {
        private readonly IDictionary<ITerminal, IUser> _terminalsUserMapp;
        private readonly IDictionary<IUser, ICollection<CallInfo>> _userCallinfoDictionary;
        private readonly IDictionary<IUser, ITariffPlan> _userTariffPlansMapp;

        private int _id;


        protected BillingSystem(ICollection<ITariffPlan> tariffPlans)
        {
            TariffPlans = tariffPlans;
            _terminalsUserMapp = new Dictionary<ITerminal, IUser>();
            _userCallinfoDictionary = new Dictionary<IUser, ICollection<CallInfo>>();
            _userTariffPlansMapp = new Dictionary<IUser, ITariffPlan>();
        }

        public ICollection<ITariffPlan> TariffPlans { get; }

        public ITerminal GetContract(IUser user, ITariffPlan tariffPlan)
        {
            var terminal = new TestTerminal(new PhoneNumber((10000 + _id).ToString()));
            terminal.TariffPlan = tariffPlan;
            _id++;

            _userTariffPlansMapp.Add(user, tariffPlan);
            _userCallinfoDictionary.Add(user, new List<CallInfo>());
            _terminalsUserMapp.Add(terminal, user);

            return terminal;
        }

        public void CallInfoHandler(object sender, CallInfo callInfo)
        {
            var sourcePair = GetUserTerminalMapPair(callInfo.Source);
            var targetPair = GetUserTerminalMapPair(callInfo.Target);
            var targetCallInfo = new CallInfo(callInfo.Target, callInfo.Source, CallInfoState.IncomingCall)
            {
                TimeBegin = callInfo.TimeBegin,
                Duration = callInfo.Duration,
                Cost = 0
            };

            callInfo.Cost = CalculateCallCost(callInfo.Duration, GeTariffPlan(sourcePair.Value));

            _userCallinfoDictionary[sourcePair.Value].Add(callInfo);
            _userCallinfoDictionary[targetPair.Value].Add(targetCallInfo);
        }

        protected virtual double CalculateCallCost(TimeSpan duration, ITariffPlan userTariffPlan)
        {
            if (userTariffPlan.FreeMinutes != 0)
            {
                if (userTariffPlan.FreeMinutes - Math.Abs(duration.TotalMinutes) < 0)
                {
                    userTariffPlan.FreeMinutes = 0;

                    return userTariffPlan.CostOneMinute*
                           (Math.Abs(duration.TotalMinutes) - userTariffPlan.FreeMinutes);
                }

                userTariffPlan.FreeMinutes -= Math.Abs(duration.TotalMinutes);

                return 0;
            }

            return userTariffPlan.CostOneMinute*Math.Abs(duration.TotalMinutes);
        }

        protected KeyValuePair<ITerminal, IUser> GetUserTerminalMapPair(PhoneNumber number)
        {
            return _terminalsUserMapp.FirstOrDefault(x => x.Key.Number == number);
        }

        protected ITariffPlan GeTariffPlan(IUser user)
        {
            return _userTariffPlansMapp.FirstOrDefault(x => x.Key == user).Value;
        }

        public void AddNewTariffPlan(ITariffPlan tariffPlan)
        {
            TariffPlans.Add(tariffPlan);
        }
    }
}