namespace ATS.Station_Model.Intarfaces
{
    public interface IStation
    {
        void RegisterEventHandlersForTerminal(ITerminal terminal);
        void RegisterEventHandlersForPort(IPort port);
    }
}