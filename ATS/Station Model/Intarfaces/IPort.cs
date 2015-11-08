using System;
using ATS.Station_Model.States;

namespace ATS.Station_Model.Intarfaces
{
    public interface IPort
    {
        PortState State { get; set; }
        event EventHandler<PortState> StateChanging;
        event EventHandler<PortState> StateChanged;
        void RegisterEventHandlersForTerminal(ITerminal terminal);
    }
}