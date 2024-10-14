using LFramework;
using UnityEngine;

namespace Game
{
    public class ParkourRace_Player : MonoBehaviour
    {
        private ParkourRace_Master _master;

        private Player _player { get { return Player.Instance; } }

        private int _checkpointIndex;

        private void Awake()
        {
            StaticBus<Event_ParkourRace_Construct_Start>.Subscribe(StaticBus_ParkourRace_Construct_Start);
            StaticBus<Event_ParkourRace_Construct_Complete>.Subscribe(StaticBus_ParkourRace_Level_Construct_Complete);

            StaticBus<Event_ParkourRace_Gameplay_Start>.Subscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ParkourRace_Construct_Start>.Unsubscribe(StaticBus_ParkourRace_Construct_Start);
            StaticBus<Event_ParkourRace_Construct_Complete>.Unsubscribe(StaticBus_ParkourRace_Level_Construct_Complete);

            StaticBus<Event_ParkourRace_Gameplay_Start>.Unsubscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void Start()
        {
            _master = GetComponent<ParkourRace_Master>();
        }

        private void StaticBus_ParkourRace_Construct_Start(Event_ParkourRace_Construct_Start e)
        {
        }

        private void StaticBus_ParkourRace_Level_Construct_Complete(Event_ParkourRace_Construct_Complete e)
        {
            _player.character.motor.SetPositionAndRotation(ParkourRace_Static.level.checkpoints[0].transformCached.position + Vector3.up * 0.5f, ParkourRace_Static.level.checkpoints[0].transformCached.rotation);
            _player.character.SetEnabled(false);

            _player.character.eventDie += Character_EventDie;

            _master.gui.progress.Add(_player.character.gameObjectCached.AddComponent<ParkourRace_Character>());
        }

        private void Character_EventDie()
        {
            AdsHelper.ShowInterstitialBreak(AdsPlacement.ParkourRace_Die);
        }

        private void StaticBus_ParkourRace_Gameplay_Start(Event_ParkourRace_Gameplay_Start e)
        {
            _player.character.SetEnabled(true);
        }
    }
}
