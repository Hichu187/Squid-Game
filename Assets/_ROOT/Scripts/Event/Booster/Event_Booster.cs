using LFramework;

namespace Game
{
    public class Event_Booster : IEvent
    {
        public bool isActive { get; private set; }
        public BoosterConfig config { get; private set; }

        public Event_Booster(BoosterConfig config, bool isActive)
        {
            this.config = config;
            this.isActive = isActive;
        }
    }
}
