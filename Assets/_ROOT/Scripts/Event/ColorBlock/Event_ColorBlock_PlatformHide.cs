using LFramework;

namespace Game
{
    public class Event_ColorBlock_PlatformHide : IEvent
    {
        public ColorBlock_ColorConfig colorConfig { get; private set; }

        public Event_ColorBlock_PlatformHide(ColorBlock_ColorConfig colorConfig)
        {
            this.colorConfig = colorConfig;
        }
    }
}
