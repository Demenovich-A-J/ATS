namespace ATS.Station_Model
{
    public interface IStation
    {
        void RegisterEventHandlersForTerminal(ITerminal terminal);
        void RegisterEventHandlersForPort(IPort port);
    }
}