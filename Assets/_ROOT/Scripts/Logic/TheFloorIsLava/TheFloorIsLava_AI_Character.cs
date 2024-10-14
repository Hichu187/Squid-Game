using LFramework;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_AI_Character : MonoBehaviour
    {
        private void Awake()
        {
            StaticBus<Event_TheFloorIsLava_LevelStart>.Subscribe(StaticBus_TheFloorIsLava_LevelStart);
        }

        private void OnDestroy()
        {
            StaticBus<Event_TheFloorIsLava_LevelStart>.Unsubscribe(StaticBus_TheFloorIsLava_LevelStart);
        }

        private void StaticBus_TheFloorIsLava_LevelStart(Event_TheFloorIsLava_LevelStart e)
        {
            gameObject.AddComponent<AIFollowWaypoint>();
        }
    }
}
