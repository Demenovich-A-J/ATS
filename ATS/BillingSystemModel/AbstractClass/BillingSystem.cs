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
        private readonly IDictionary<IUser, ITariffPlan> _userTariffPlansMapp;
        private IDictionary<IUser, DateTime> _userPayDateTime; 

        private int _id;

        protected BillingSystem(ICollection<ITariffPlan> tariffPlans)
        {
            TariffPlans = tariffPlans;
            _terminalsUserMapp = new Dictionary<ITerminal, IUser>();
            UserCallinfoDictionary = new Dictionary<IUser, ICollection<CallInfo>>();
            _userTariffPlansMapp = new Dictionary<IUser, ITariffPlan>();
            UserRegistryDateTimeMapp = new Dictionary<IUser, DateTime>();
            _userPayDateTime = new Dictionary<IUser, DateTime>();
        }

        protected IDictionary<IUser, ICollection<CallInfo>> UserCallinfoDictionary { get; }

        public IDictionary<IUser, DateTime> UserRegistryDateTimeMapp { get; }

        public ICollection<ITariffPlan> TariffPlans { get; }

        public IDictionary<IUser, DateTime> UserPayDateTime
        {
            get { return _userPayDateTime; }
            set { _userPayDateTime = value; }
        }


        public ITerminal GetContract(IUser user, ITariffPlan tariffPlan)
        {
            var terminal = new TestTerminal(new PhoneNumber((10000 + _id).ToString()), tariffPlan);
            _id++;

            _userTariffPlansMapp.Add(user, tariffPlan);
            UserCallinfoDictionary.Add(user, new List<CallInfo>());
            _terminalsUserMapp.Add(terminal, user);
            var date = TimeHelper.Now;
            UserRegistryDateTimeMapp.Add(user, date);
            _userPayDateTime.Add(user, date);
            OnToSignСontract(terminal);
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

            UserCallinfoDictionary[sourcePair.Value].Add(callInfo);
            UserCallinfoDictionary[targetPair.Value].Add(targetCallInfo);
        }



        public void SetNewTariffPlan(IUser user, ITariffPlan tariffPlan)
        {
            var contractDate = GetContracDateTime(user);
            if (contractDate > TimeHelper.Now)
            {
                _userTariffPlansMapp.Remove(user);
                _userTariffPlansMapp.Add(user, tariffPlan);
                UserRegistryDateTimeMapp.Remove(user);
                UserRegistryDateTimeMapp.Add(user, TimeHelper.Now);
            }
            else
            {
                Console.WriteLine("Month has not yet passed");
            }
        }

        protected DateTime GetContracDateTime(IUser user)
        {
            return UserRegistryDateTimeMapp.FirstOrDefault(x => x.Key == user).Value;
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

        public event EventHandler<ITerminal> ToSignСontract; 
        public event EventHandler<IUser> Pay;

        public void PayForPhoneNubmer(PhoneNumber phoneNumber)
        {
            OnPay(_terminalsUserMapp.FirstOrDefault(x => x.Key.Number == phoneNumber).Value);
        }

        protected virtual void OnPay(IUser user)
        {
            Pay?.Invoke(this, user);
        }


        protected virtual void OnToSignСontract(ITerminal terminal)
        {
            ToSignСontract?.Invoke(this, terminal);
        }
    }
}