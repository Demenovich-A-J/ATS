using System;
using System.Collections.Generic;
using System.Linq;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Helpers;
using ATS.Station_Model.Intarfaces;
using ATS.Test;
using ATS.User_Model;

namespace ATS.BillingSystemModel.AbstractClass
{
    public abstract class BillingSystem : IBillingSystem
    {
        private readonly IDictionary<ITerminal, IUser> _terminalsUserMapp;
        private readonly IDictionary<IUser, ICollection<CallInfo>> _userCallinfoDictionary;
        private readonly IDictionary<IUser, ITariffPlan> _userTariffPlansMapp;
        private readonly IDictionary<IUser, DateTime> _userDateTimesMapp; 

        private int _id;


        protected BillingSystem(ICollection<ITariffPlan> tariffPlans)
        {
            TariffPlans = tariffPlans;
            _terminalsUserMapp = new Dictionary<ITerminal, IUser>();
            _userCallinfoDictionary = new Dictionary<IUser, ICollection<CallInfo>>();
            _userTariffPlansMapp = new Dictionary<IUser, ITariffPlan>();
            _userDateTimesMapp = new Dictionary<IUser, DateTime>();
        }

        public ICollection<ITariffPlan> TariffPlans { get; }

        protected IDictionary<IUser, ICollection<CallInfo>> UserCallinfoDictionary => _userCallinfoDictionary;

        public ITerminal GetContract(IUser user, ITariffPlan tariffPlan)
        {
            var terminal = new TestTerminal(new PhoneNumber((10000 + _id).ToString()), tariffPlan);
            _id++;

            _userTariffPlansMapp.Add(user, tariffPlan);
            UserCallinfoDictionary.Add(user, new List<CallInfo>());
            _terminalsUserMapp.Add(terminal, user);
            _userDateTimesMapp.Add(user, TimeHelper.Now);

            return terminal;
        }

        public void SetNewTariffPlan(IUser user, ITariffPlan tariffPlan)
        {
            var contractDate = GetContracDateTime(user);
            if (contractDate > TimeHelper.Now)
            {
                _userTariffPlansMapp.Remove(user);
                _userTariffPlansMapp.Add(user, tariffPlan);
                _userDateTimesMapp.Remove(user);
                _userDateTimesMapp.Add(user, TimeHelper.Now);
            }
            else
            {
                Console.WriteLine("Month has not yet passed");
            }
        }

        protected DateTime GetContracDateTime(IUser user)
        {
            return _userDateTimesMapp.FirstOrDefault(x => x.Key == user).Value;
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

            UserCallinfoDictionary[sourcePair.Value].Add(callInfo);
            UserCallinfoDictionary[targetPair.Value].Add(targetCallInfo);
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