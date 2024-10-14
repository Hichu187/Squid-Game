using DG.Tweening;
using LFramework;
using System;
using UnityEngine;

namespace Game
{
    public class EasyObby_Character : MonoBehaviour
    {
        private EasyObby_Checkpoint _checkpointCurrent;
        private EasyObby_Checkpoint _checkpointNext;

        private Character _character;

        private Tween _tween;

        public Character character
        {
            get
            {
                if (_character == null)
                    _character = GetComponent<Character>();

                return _character;
            }
        }

        public EasyObby_Checkpoint checkpointCurrent
        {
            get { return _checkpointCurrent; }
            set
            {
                _checkpointCurrent = value;

                _checkpointNext = EasyObby_StageManager.checkpoints.GetClamp(_checkpointCurrent.index + 1);

                eventCheckpointChanged?.Invoke();
            }
        }

        public EasyObby_Checkpoint checkpointNext { get { return _checkpointNext; } }

        public event Action eventCheckpointChanged;

        private void Awake()
        {
            StaticBus<Event_EasyObby_Checkpoint>.Subscribe(StaticBus_EasyObby_Checkpoint);
        }

        private void OnDestroy()
        {
            StaticBus<Event_EasyObby_Checkpoint>.Unsubscribe(StaticBus_EasyObby_Checkpoint);
        }

        private void Start()
        {
            character.eventDie += Character_EventDie;
        }

        private void Character_EventDie()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(FactoryEasyObby.reviveDelay, Revive, false);
        }

        private void StaticBus_EasyObby_Checkpoint(Event_EasyObby_Checkpoint e)
        {
            if (e.character != character)
                return;

            if (checkpointCurrent == null || checkpointCurrent.index < e.checkpoint.index)
                checkpointCurrent = e.checkpoint;
        }

        public void Revive()
        {
            // Revive character on checkpoint
            character.Revive(checkpointCurrent.transform.position + Vector3.up, checkpointCurrent.transform.rotation);
        }
    }
}
