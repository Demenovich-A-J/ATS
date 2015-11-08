using System;

namespace ATS
{
    public class CallInfo
    {
        private readonly PhoneNumber _target;
        private readonly PhoneNumber _source;

        public CallInfo(PhoneNumber target, PhoneNumber source)
        {
            _target = target;
            _source = source;
        }

        public PhoneNumber Target => _target;

        public PhoneNumber Source => _source;

        public DateTime TimeBegin { get; set; }

        public TimeSpan Duration { get; set; }
    }
}