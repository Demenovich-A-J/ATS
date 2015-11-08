using System;
using ATS.Station_Model.States;

namespace ATS.Station_Model.Intarfaces
{
    public interface ITerminal
    {
        PhoneNumber Number { get; }
        event EventHandler<CallInfo> OutgoingCall;
        event EventHandler<ResponseState> Responce;
        event EventHandler Pluging;
        event EventHandler UnPluging;
        event EventHandler Online;
        event EventHandler Ofline;

        void Drop();
        void Answer();
        void Plug();
        void Unplug();
        void Call(PhoneNumber target);
    }
}