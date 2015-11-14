using System;
using System.Threading;

namespace ATS.Helpers
{
    public static class TimeHelper
    {
        private static int _counter = 0;

        public static DateTime Now
        {
            get
            {
                _counter++;
                return DateTime.Now.Add(new TimeSpan(5 * _counter, 0, 10 * _counter, 5 * _counter));
            }
        }

        public static TimeSpan Duration()
        {
            return new TimeSpan(0, new Random().Next(0, 59), new Random().Next(0, 59));
        }
    }
}