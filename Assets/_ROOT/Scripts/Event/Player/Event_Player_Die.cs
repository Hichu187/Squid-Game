using LFramework;

namespace Game
{
    public class Event_Player_Die : IEvent
    {
        public Character character { get; private set; }

        public Event_Player_Die(Character character)
        {
            this.character = character;
        }
    }
}
