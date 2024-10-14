using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ParkourRace_AIManager : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _aiPrefab;

        [Space]

        [SerializeField] private Vector2 _idleDurationRange = new Vector2(0.1f, 0.5f);

        private AI[] _ai;

        private ParkourRace_Master _master;

        private void Awake()
        {
            _master = GetComponent<ParkourRace_Master>();

            InitAI();

            StaticBus<Event_ParkourRace_Construct_Start>.Subscribe(StaticBus_ParkourRace_Construct_Start);
            StaticBus<Event_ParkourRace_Construct_Complete>.Subscribe(StaticBus_ParkourRace_Construct_Complete);

            StaticBus<Event_ParkourRace_Gameplay_Start>.Subscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ParkourRace_Construct_Start>.Unsubscribe(StaticBus_ParkourRace_Construct_Start);
            StaticBus<Event_ParkourRace_Construct_Complete>.Unsubscribe(StaticBus_ParkourRace_Construct_Complete);

            StaticBus<Event_ParkourRace_Gameplay_Start>.Unsubscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void InitAI()
        {
            _ai = new AI[2];

            for (int i = 0; i < 2; i++)
            {
                _ai[i] = _aiPrefab.Create().GetComponent<AI>();

                _ai[i].SetIdleDurationRange(_idleDurationRange);
            }
        }

        private void StaticBus_ParkourRace_Construct_Start(Event_ParkourRace_Construct_Start e)
        {
        }

        private void StaticBus_ParkourRace_Construct_Complete(Event_ParkourRace_Construct_Complete e)
        {
            ParkourRace_Checkpoint checkpoint = ParkourRace_Static.level.checkpoints[0];

            for (int i = 0; i < 2; i++)
            {
                _ai[i].character.motor.SetPositionAndRotation(checkpoint.transform.position + Vector3.left + i * Vector3.right * 2f + Vector3.up, checkpoint.transform.rotation);

                _master.gui.progress.Add(_ai[i].character.gameObject.AddComponent<ParkourRace_Character>());
            }
        }

        private void StaticBus_ParkourRace_Gameplay_Start(Event_ParkourRace_Gameplay_Start e)
        {
            for (int i = 0; i < 2; i++)
            {
                _ai[i].gameObject.AddComponent<AIFollowWaypoint>();
            }
        }
    }
}
