using LFramework;

namespace Game
{
    public class Event_TheFloorIsLava_Result : IEvent
    {
        public bool isWin { get; private set; }

        public Event_TheFloorIsLava_Result(bool isWin)
        {
            this.isWin = isWin;
        }
    }
}
