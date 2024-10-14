using LFramework;

namespace Game
{
    public class Event_Player_Revive : IEvent
    {
        public Character character { get; private set; }

        public Event_Player_Revive(Character character)
        {
            this.character = character;
        }
    }
}
