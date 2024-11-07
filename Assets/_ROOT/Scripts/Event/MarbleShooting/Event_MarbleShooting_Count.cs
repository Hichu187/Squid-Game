using LFramework;

namespace Game
{
    public class Event_MarbleShooting_Count : IEvent
    {
        public MarbleShooting_Marble _marble { get; private set; }

        public bool _isPlayer {  get; private set; }

        public Event_MarbleShooting_Count(bool isPlayer, MarbleShooting_Marble marble)
        {
            _isPlayer = isPlayer;
            _marble = marble;
        }
    }
}
