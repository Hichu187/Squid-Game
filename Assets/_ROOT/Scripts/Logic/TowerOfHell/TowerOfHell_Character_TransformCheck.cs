using System;
using UnityEngine;
using Vertx.Debugging;

namespace Game
{
    public class TowerOfHell_Character_TransformCheck : MonoBehaviour
    {
        private Vector3 _lastStablePosition;
        private Quaternion _lastStableRotation;

        private Character _character;

        public Vector3 lastStablePosition { get { return _lastStablePosition; } }
        public Quaternion lastStableRotation { get { return _lastStableRotation; } }

        public event Action<TowerOfHell_Character_TransformCheck> eventTransformStable;

        private void Start()
        {
            _character = GetComponent<Character>();

            _lastStablePosition = _character.transformCached.position;
            _lastStableRotation = _character.transformCached.rotation;
        }

        private void Update()
        {
            if (_character.motor.GroundingStatus.IsStableOnGround)
            {
                _lastStablePosition = _character.transformCached.position;
                _lastStableRotation = _character.transformCached.rotation;

                eventTransformStable?.Invoke(this);
            }
        }

        private void OnDrawGizmos()
        {
            D.raw(new Shape.Capsule(_lastStablePosition + Vector3.up * 0.85f, _lastStableRotation, 1.7f, 0.3f), Color.green);
        }
    }
}
