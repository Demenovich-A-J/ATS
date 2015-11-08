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
                return Number == ((PhoneNumber) obj).Number;
            }

            return false;
        }

        public bool Equals(PhoneNumber other)
        {
            return string.Equals(Number, other.Number);
        }

        public override int GetHashCode()
        {
            return Number?.GetHashCode() ?? 0;
        }

        public static bool operator == (PhoneNumber x, PhoneNumber y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PhoneNumber x, PhoneNumber y)
        {
            return !(x == y);
        }
    }
}