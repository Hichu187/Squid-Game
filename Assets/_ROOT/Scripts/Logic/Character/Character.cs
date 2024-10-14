using KinematicCharacterController;
using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game
{
    public class Character : MonoCached
    {
        public enum State
        {
            Normal,
            Die,
        }

        [Title("Reference")]
        [SerializeField] private KinematicCharacterMotor _motor;
        [SerializeField] private CharacterKC _controller;
        [SerializeField] private CharacterRenderer _renderer;
        [SerializeField] private CharacterAnimator _animator;

        [Title("Config")]
        [SerializeField] private bool _isPlayer;

        private State _state = State.Normal;

        private bool _boosterJetpackEnabled = false;
        private bool _boosterShoesEnabled = false;

        public bool isPlayer { get { return _isPlayer; } }
        public CharacterKC controller { get { return _controller; } }
        public KinematicCharacterMotor motor { get { return _motor; } }
        public bool boosterJetpackEnabled { get { return _boosterJetpackEnabled; } }
        public bool boosterShoesEnabled { get { return _boosterShoesEnabled; } }
        public CharacterRenderer rendererComp { get { return _renderer; } }
        public CharacterAnimator animator { get { return _animator; } }

        public event Action eventDie;
        public event Action eventRevive;
        public event Action eventBoosterUpdate;

        private void OnCollisionEnter(Collision collision)
        {
            ICharacterCollidable collidable = collision.collider.GetComponentInParent<ICharacterCollidable>();

            if (collidable != null)
                collidable.OnCollisionEnter(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            ICharacterCollidable collidable = other.GetComponentInParent<ICharacterCollidable>();

            if (collidable != null)
                collidable.OnTriggerEnter(this);
        }

        public void ActiveBooster(BoosterType type)
        {
            switch (type)
            {
                case BoosterType.JetPack:
                    _boosterJetpackEnabled = true;
                    break;
                case BoosterType.Shoes:
                    _boosterShoesEnabled = true;
                    _controller.jumpUpSpeedMultiple = FactoryConfig.characterBoosterJumpMultiple;
                    break;
            }

            eventBoosterUpdate?.Invoke();
        }

        public void DeactiveBooster()
        {
            _boosterJetpackEnabled = false;
            _boosterShoesEnabled = false;
            _controller.jumpUpSpeedMultiple = 1.0f;

            eventBoosterUpdate?.Invoke();
        }

        public void Kill()
        {
            Die();
        }

        private void Die()
        {
            if (_state == State.Die)
                return;

            _state = State.Die;

            SetEnabled(false);

            eventDie?.Invoke();
        }

        public void Revive(Vector3 position, Quaternion rotation)
        {
            _state = State.Normal;

            _motor.SetPositionAndRotation(position, rotation);

            SetEnabled(true);

            eventRevive?.Invoke();
        }

        public void Win()
        {
            SetEnabled(false);

            _animator.PlayWin();
        }

        public void DisableDieByFalling()
        {
            CharacterFallDetector fallDetector = gameObjectCached.GetComponent<CharacterFallDetector>();

            if (fallDetector != null)
                Destroy(fallDetector);
        }

        public void SetEnabled(bool enabled)
        {
            _motor.enabled = enabled;
            _motor.GetComponent<Rigidbody>().isKinematic = !enabled;
        }
    }
}
