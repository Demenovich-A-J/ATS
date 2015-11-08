using ATS.Station_Model.States;

namespace ATS
{
    public class Responce
    {
        public Responce(ResponseState state, PhoneNumber source)
        {
            State = state;
            Source = source;
        }

        public ResponseState State { get; }
        public PhoneNumber Source { get; }
    }
}