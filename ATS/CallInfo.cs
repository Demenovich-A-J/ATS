using System;

namespace ATS
{
    public class CallInfo
    {
        public CallInfo(PhoneNumber target, PhoneNumber source, CallInfoState state)
        {
            Target = target;
            Source = source;
            State = state;
        }

        public PhoneNumber Target { get; }

        public PhoneNumber Source { get; }

        public DateTime TimeBegin { get; set; }

        public TimeSpan Duration { get; set; }
        public CallInfoState State { get; }
        public double Cost { get; set; }
    }
}