using System;

namespace ATS.Station_Model
{
    public interface ITerminal
    {
        PhoneNumber Number { get; }
        event EventHandler OutgoingCall;
        event EventHandler IncomingCall;
        event EventHandler Plugging;
        event EventHandler UnPluging;
        event EventHandler Online;
        event EventHandler Ofline;

        void Drop();
        void Answer();
        void Plug();
        void Unplug();
        void Call(PhoneNumber target);
        void IncomingCallFrom(PhoneNumber source);
    }
}