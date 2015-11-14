using System;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Station_Model.AbstractClasses
{
    public abstract class Terminal : ITerminal
    {
        protected Terminal(PhoneNumber number, ITariffPlan tariffPlan)
        {
            Number = number;
            TariffPlan = tariffPlan;
        }

        private bool IsOnline { get; set; }
        private bool IncomeCall { get; set; }
        public PhoneNumber Number { get; }
        public ITariffPlan TariffPlan { get; set; }

        public event EventHandler<CallInfo> OutgoingCall;
        public event EventHandler<Response> Responce;
        public event EventHandler<PhoneNumber> IncomingRequest;

        public void GetReqest(PhoneNumber source)
        {
            IncomeCall = true;
            OnIncomingRequest(source);
        }


        protected virtual void OnIncomingRequest(PhoneNumber source)
        {
            IncomingRequest?.Invoke(this, source);
        }

        public void Drop()
        {
            if(!IncomeCall) return;
            OnResponce(this, new Response(ResponseState.Drop, Number));
            IncomeCall = false;
        }

        public void Answer()
        {
            if (!IncomeCall) return;
            OnResponce(this, new Response(ResponseState.Accept, Number));
        }

        public void Reject()
        {
            if (!IncomeCall) return;
            OnResponce(this, new Response(ResponseState.Reject, Number));
            IncomeCall = false;
        }

        public void Call(PhoneNumber target)
        {
            if (IsOnline)
            {
                OnOutgoingCall(this, new CallInfo(target, Number,CallInfoState.OutGoingCall));
            }
        }

        public event EventHandler Online;
        public event EventHandler Offline;

        public event EventHandler Plugging;
        public event EventHandler UnPlugging;

        public void Plug()
        {
            if(IsOnline) return;
            OnPlugging(this, null);
            IsOnline = true;
        }

        public void Unplug()
        {
            if(IsOnline == false) return;
            OnUnPlugging(this, null);
            IsOnline = false;
        }

        public virtual void RegisterEventHandlersForPort(IPort port)
        {
            port.StateChanged += (sender, state) =>
            {
                if(state == PortState.Unpluged)
                    OnOffline(sender, null);
                if(state == PortState.Free)
                    OnOnline(sender,null);
            };
        }

        protected virtual void OnResponce(object sender, Response responce)
        {
            Responce?.Invoke(this, responce);
        }


        protected virtual void OnOutgoingCall(object sender, CallInfo target)
        {
            OutgoingCall?.Invoke(this, target);
        }

        protected virtual void OnOnline(object sender, EventArgs args)
        {
            Online?.Invoke(this, args);
            IsOnline = true;
        }

        protected virtual void OnOffline(object sender, EventArgs args)
        {
            Offline?.Invoke(this, args);
            IsOnline = false;
        }

        protected virtual void OnPlugging(object sender, EventArgs args)
        {
            Plugging?.Invoke(this, args);
        }

        protected virtual void OnUnPlugging(object sender, EventArgs args)
        {
            UnPlugging?.Invoke(this, args);
        }
    }
}