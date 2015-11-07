using System;

namespace ATS.Station_Model
{
    public interface IPort
    {
        PortState State { get; }
        event EventHandler<PortState> StateChanging;
        event EventHandler<PortState> StateChanged;
        
    }
}