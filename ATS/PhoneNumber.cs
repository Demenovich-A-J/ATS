using System;

namespace ATS
{
    public struct PhoneNumber
    {
        private readonly string _number;

        public PhoneNumber(string number)
        {
            _number = number;
        }

        public string Number => _number;

        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber)
            {
                return _number == ((PhoneNumber) obj)._number;
            }

            return false;
        }

        public bool Equals(PhoneNumber other)
        {
            return string.Equals(_number, other._number);
        }

        public override int GetHashCode()
        {
            return _number?.GetHashCode() ?? 0;
        }
    }
}