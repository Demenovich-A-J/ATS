using System;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Station_Model.AbstractClasses
{
    public abstract class Terminal : ITerminal
    {
        protected Terminal(PhoneNumber number)
        {
            Number = number;
        }
        protected bool IsOnline { get; private set; }
        public PhoneNumber Number { get; }

        public event EventHandler<CallInfo> OutgoingCall;
        public event EventHandler<ResponseState> Responce;

        protected virtual void OnResponce(object sender,ResponseState args)
        {
            Responce?.Invoke(this, args);
        }

        public void Drop()
        {
            OnResponce(this,ResponseState.Drop);
        }

        public void Answer()
        {
            OnResponce(this, ResponseState.Accept);
        }

        public void Call(PhoneNumber target)
        {
            if (IsOnline)
            {
                OnOutgoingCall(this,new CallInfo(target, Number, DateTime.Now));
            }
        }


        protected virtual void OnOutgoingCall(object sender,CallInfo target)
        {
            OutgoingCall?.Invoke(this, target);
        }

        public event EventHandler Online;
        public event EventHandler Ofline;

        protected virtual void OnOnline(object sender, EventArgs args)
        {
            Online?.Invoke(this, args);
            IsOnline = true;
        }

        protected virtual void OnOfline(object sender, EventArgs args)
        {
            Ofline?.Invoke(this, args);
            IsOnline = false;
        }

        public event EventHandler Pluging;
        public event EventHandler UnPluging;

        protected virtual void OnPluging(object sender, EventArgs args)
        {
            Pluging?.Invoke(this, args);
        }

        protected virtual void OnUnPluging(object sender, EventArgs args)
        {
            UnPluging?.Invoke(this, args);
        }

        public void Plug()
        {
            OnPluging(this, null);
        }

        public void Unplug()
        {
            if(!IsOnline) return;

            OnUnPluging(this,null);
        }
    }
}