using LFramework;

namespace Game
{
    public class Event_RedLightGreenLight_GreenLight : IEvent
    {
        public float countDown { get; private set; }

        public Event_RedLightGreenLight_GreenLight(float countDown)
        {
            this.countDown = countDown;
        }
    }
}
