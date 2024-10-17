using LFramework;

namespace Game
{
    public class Event_MarbleShooting_Count : IEvent
    {
        public bool _isPlayer {  get; private set; }

        public Event_MarbleShooting_Count(bool isPlayer)
        {
            _isPlayer = isPlayer;
        }
    }
}
