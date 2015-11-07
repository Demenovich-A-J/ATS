using System.Collections.Generic;

namespace ATS.Station_Model
{
    public abstract class Station : IStation
    {
        private List<IPort> _ports;
        private List<ITerminal> _terminals;

        protected Station(List<IPort> ports, List<ITerminal> terminals)
        {
            _ports = ports;
            _terminals = terminals;
        }

        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        public abstract void RegisterEventHandlersForPort(IPort port);
    }
}