using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class BladeBall_AI_Manager : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _aiPrefab;
        [SerializeField] private RuntimeAnimatorController _animator;
        [SerializeField] private Vector2 _aiIdleDurationRange = new Vector2(0f, 0.5f);
        [SerializeField] private Vector2 _aiRandomPositionRange;
        public BladeBall_Map _map;
        private AI[] _ai;
        private BladeBall_Ball _ball;

        [Title("SFX")]
        [SerializeField] AudioConfig _slash;
        [SerializeField] AudioConfig _atkSfx;
        [SerializeField] GameObject _vfxSlash;
        private void Awake()
        {
            _map = FindFirstObjectByType<BladeBall_Map>();
            _ball = FindFirstObjectByType<BladeBall_Ball>();
            StaticBus<Event_BladeBall_LevelConstructed>.Subscribe(StaticBus_BladeBall_LevelConstructed);
            StaticBus<Event_BladeBall_Result>.Subscribe(StaticBus_BladeBall_Result);

        }

        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_LevelConstructed>.Unsubscribe(StaticBus_BladeBall_LevelConstructed);
            StaticBus<Event_BladeBall_Result>.Unsubscribe(StaticBus_BladeBall_Result);
        }

        private void StaticBus_BladeBall_LevelConstructed(Event_BladeBall_LevelConstructed e)
        {
            _ai = new AI[e.ais.Count];

            for (int i = 0; i < e.ais.Count; i++)
            {
                AI ai = _aiPrefab.Create().GetComponent<AI>();

                ai.gameObject.AddComponent<AIFollowWaypoint>();
                ai.GetComponent<AIFollowWaypoint>().enabled = false;

                Character character = ai.transform.GetChild(0).GetComponent<Character>();

                character.gameObject.AddComponent<BladeBall_AI_Character>();
                character.GetComponent<BladeBall_AI_Character>()._animator = _animator;

                if (DataBladeBall.loseCount >= 2)
                {
                    if (e.ais[i] != AIType.Normal_1 && e.ais[i] != AIType.Target_1)
                    {
                        character.GetComponent<BladeBall_AI_Character>().aiType = e.ais[i] - 1;
                    }
                    else
                    {
                        character.GetComponent<BladeBall_AI_Character>().aiType = e.ais[i];
                    }
                }
                else
                {
                    character.GetComponent<BladeBall_AI_Character>().aiType = e.ais[i];
                }

                character.GetComponent<BladeBall_AI_Character>().SetHit();
                character.GetComponent<BladeBall_AI_Character>()._slash = _slash;
                character.GetComponent<BladeBall_AI_Character>()._vfxSlash = _vfxSlash;
                character.GetComponent<BladeBall_AI_Character>()._atkSfx = _atkSfx;

                float idleDuration = _aiIdleDurationRange.RandomWithin();
                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));
                _ai[i] = ai;
                _ball._listTargets.Add(ai.transform.GetChild(0));
                GetComponent<BladeBall_Manager>()._enem.Add(ai.transform.GetChild(0));
                ai.character.Revive(_map.AiStartPosition[i].position, Quaternion.LookRotation(_map.AiStartPosition[i].position.normalized, Vector3.up));

            }
        }

        private void StaticBus_BladeBall_Result(Event_BladeBall_Result e)
        {

        }
    }
}
