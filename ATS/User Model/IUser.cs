using ATS.Station_Model.Intarfaces;

namespace ATS.User_Model
{
    public interface IUser
    {
        string Name { get; }
        ITerminal Phone { get; }
        void Call(PhoneNumber target);
        void Drop();
        void Answer();
        void Plug();
        void UnPlug();
    }
}