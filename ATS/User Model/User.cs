using ATS.Station_Model.Intarfaces;

namespace ATS.User_Model
{
    public abstract class User : IUser
    {
        protected User(string name, ITerminal phone)
        {
            Name = name;
            Phone = phone;
        }

        public string Name { get; }
        public ITerminal Phone { get; }

        public virtual void Call(PhoneNumber target)
        {
            Phone.Call(target);
        }

        public virtual void Drop()
        {
            Phone.Drop();
        }

        public virtual void Answer()
        {
            Phone.Answer();
        }

        public virtual void Plug()
        {
            Phone.Plug();
        }

        public void UnPlug()
        {
            Phone.Unplug();
        }
    }
}