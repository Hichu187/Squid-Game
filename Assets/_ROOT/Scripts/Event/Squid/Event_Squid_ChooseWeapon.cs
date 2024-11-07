using LFramework;

namespace Game
{
    public class Event_Squid_ChooseWeapon : IEvent
    {
        public int id { get; private set; }
        public Event_Squid_ChooseWeapon(int id)
        {
            this.id = id;
        }

    }
}
