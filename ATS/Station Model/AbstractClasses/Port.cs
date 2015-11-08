using System;
using ATS.Station_Model.Intarfaces;
using ATS.Station_Model.States;

namespace ATS.Station_Model.AbstractClasses
{
    public abstract class Port : IPort
    {
        private PortState _state = PortState.Unpluged;

        public PortState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;

                OnStateChanging(this, value);
                _state = value;
                OnStateChanged(this, _state);
            }
        }

        public event EventHandler<PortState> StateChanging;
        public event EventHandler<PortState> StateChanged;
        public abstract void RegisterEventHandlersForTerminal(ITerminal terminal);

        protected virtual void OnStateChanging(object sender, PortState newstate)
        {
            StateChanging?.Invoke(this, newstate);
        }

        protected virtual void OnStateChanged(object sender, PortState state)
        {
            StateChanged?.Invoke(this, state);
        }
    }
}