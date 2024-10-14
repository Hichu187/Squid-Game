using LFramework;

namespace Game
{
    public class Event_ParkourRace_Checkpoint : IEvent
    {
        public ParkourRace_Checkpoint checkpoint { get; private set; }
        public Character character { get; private set; }

        public Event_ParkourRace_Checkpoint(ParkourRace_Checkpoint checkpoint, Character character)
        {
            this.checkpoint = checkpoint;
            this.character = character;
        }
    }
}
