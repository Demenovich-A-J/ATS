using System;

namespace ATS
{
    public class CallInfo
    {
        private readonly PhoneNumber _target;
        private readonly PhoneNumber _source;
        private readonly DateTime _timeBegin;

        public CallInfo(PhoneNumber target, PhoneNumber source, DateTime timeBegin)
        {
            _target = target;
            _source = source;
            _timeBegin = timeBegin;
        }

        public PhoneNumber Target => _target;

        public PhoneNumber Source => _source;

        public DateTime TimeBegin => _timeBegin;

        public TimeSpan Duration { get; set; }
    }
}