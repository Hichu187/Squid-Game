using LFramework;

namespace Game
{
    public class Event_EasyObby_Checkpoint : IEvent
    {
        public EasyObby_Checkpoint checkpoint { get; private set; }
        public Character character { get; private set; }

        public Event_EasyObby_Checkpoint(EasyObby_Checkpoint checkpoint, Character character)
        {
            this.checkpoint = checkpoint;
            this.character = character;
        }
    }
}
