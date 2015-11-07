using System;

namespace ATS.Station_Model
{
    public abstract class Terminal : ITerminal
    {
        public PhoneNumber Number { get; }
        public event EventHandler OutgoingCall;
        public event EventHandler IncomingCall;
        public event EventHandler Plugging;
        public event EventHandler UnPluging;
        public event EventHandler Online;
        public event EventHandler Ofline;
        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void Answer()
        {
            throw new NotImplementedException();
        }

        public void Plug()
        {
            throw new NotImplementedException();
        }

        public void Unplug()
        {
            throw new NotImplementedException();
        }

        public void Call(PhoneNumber target)
        {
            throw new NotImplementedException();
        }

        public void IncomingCallFrom(PhoneNumber source)
        {
            throw new NotImplementedException();
        }
    }
}