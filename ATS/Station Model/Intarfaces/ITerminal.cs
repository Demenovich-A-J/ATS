using System;
using ATS.Station_Model.States;

namespace ATS.Station_Model.Intarfaces
{
    public interface ITerminal
    {
        PhoneNumber Number { get; }
        event EventHandler<CallInfo> OutgoingCall;
        event EventHandler<Responce> Responce;
        event EventHandler<PhoneNumber> IncomingRequest;
        event EventHandler Plugging;
        event EventHandler UnPlugging;
        event EventHandler Online;
        event EventHandler Offline;

        void GetReqest(PhoneNumber source);
        void Drop();
        void Answer();
        void Plug();
        void Unplug();
        void Call(PhoneNumber target);

        void RegisterEventHandlersForPort(IPort port);
    }
}