using DG.Tweening;
using LFramework;
using UnityEngine;

namespace Game
{
    public class ParkourRace_Character : MonoBehaviour
    {
        private ParkourRace_Checkpoint _checkpoint;

        private Character _character;

        private Tween _tween;

        public Character character { get { return _character; } }

        public ParkourRace_Checkpoint checkpoint { get { return _checkpoint; } }

        private void Awake()
        {
            _character = GetComponent<Character>();
            _checkpoint = ParkourRace_Static.level.checkpoints[0];

            StaticBus<Event_ParkourRace_Checkpoint>.Subscribe(StaticBus_ParkourRace_Checkpoint);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ParkourRace_Checkpoint>.Unsubscribe(StaticBus_ParkourRace_Checkpoint);
        }

        private void Start()
        {
            _character.eventDie += Character_EventDie;
        }

        private void Character_EventDie()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(FactoryConfig.parkourRaceReviveDelay, () =>
            {
                // Revive character on checkpoint or default position
                if (_checkpoint != null)
                    _character.Revive(_checkpoint.transform.position + Vector3.up, _checkpoint.transform.rotation);
                else
                    _character.Revive(Vector3.up, Quaternion.identity);

            }, false);
        }

        private void StaticBus_ParkourRace_Checkpoint(Event_ParkourRace_Checkpoint e)
        {
            if (e.character != _character)
                return;

            if (_checkpoint == null || _checkpoint.index < e.checkpoint.index)
            {
                _checkpoint = e.checkpoint;

                if (_character.isPlayer && _checkpoint.index > 0)
                    _checkpoint.PlayFX();
            }
        }
    }
}
