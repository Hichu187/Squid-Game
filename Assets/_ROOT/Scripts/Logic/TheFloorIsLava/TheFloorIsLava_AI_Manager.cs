using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_AI_Manager : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _aiPrefab;
        [SerializeField] private int _count = 5;
        [SerializeField] private Vector2 _aiIdleDurationRange = new Vector2(0f, 0.5f);
        [SerializeField] private Vector2 _aiRandomPositionRange;

        private AI[] _ai;

        private void Awake()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Subscribe(StaticBus_TheFloorIsLava_LevelConstructed);
            StaticBus<Event_TheFloorIsLava_Result>.Subscribe(StaticBus_TheFloorIsLava_Result);
        }

        private void OnDestroy()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Unsubscribe(StaticBus_TheFloorIsLava_LevelConstructed);
            StaticBus<Event_TheFloorIsLava_Result>.Unsubscribe(StaticBus_TheFloorIsLava_Result);
        }

        private void StaticBus_TheFloorIsLava_LevelConstructed(Event_TheFloorIsLava_LevelConstructed e)
        {
            TheFloorIsLava_Level level = TheFloorIsLava_Static.level;

            _ai = new AI[_count];

            for (int i = 0; i < _count; i++)
            {
                AI ai = _aiPrefab.Create().GetComponent<AI>();

                ai.character.Revive(level.points.spawnPoint.position + new Vector3(_aiRandomPositionRange.RandomWithin(), 0f, _aiRandomPositionRange.RandomWithin()), level.points.spawnPoint.rotation);

                ai.gameObject.AddComponent<TheFloorIsLava_AI_Character>();

                float idleDuration = _aiIdleDurationRange.RandomWithin();

                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));

                _ai[i] = ai;
            }
        }

        private void StaticBus_TheFloorIsLava_Result(Event_TheFloorIsLava_Result e)
        {
            if (!e.isWin)
                return;

            for (int i = 0; i < _ai.Length; i++)
            {
                _ai[i].Win();
            }
        }
    }
}
