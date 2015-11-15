using System;

namespace ATS.Station_Model.Intarfaces
{
    public interface IStation : IShouldClearEventHandlers
    {
        event EventHandler<CallInfo> CallInfoPrepared; 
        void RegisterEventHandlersForTerminal(ITerminal terminal);
        void RegisterEventHandlersForPort(IPort port);
    }
}