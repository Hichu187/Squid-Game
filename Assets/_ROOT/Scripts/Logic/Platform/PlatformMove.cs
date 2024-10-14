using DG.Tweening;
using KinematicCharacterController;
using LFramework;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PhysicsMover))]
    public class PlatformMove : MonoCached, IMoverController
    {
        AnimationSequence _animationSequence;

        Sequence _sequence;

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        private void Start()
        {
            _animationSequence = GetComponentInChildren<AnimationSequence>();

            GetComponent<PhysicsMover>().MoverController = this;
        }

        // This is called every FixedUpdate by our PhysicsMover in order to tell it what pose it should go to
        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            // Remember pose before animation
            Vector3 _positionBeforeAnim = transformCached.position;
            Quaternion _rotationBeforeAnim = transformCached.rotation;

            // Update animation
            _animationSequence.sequence.ManualUpdate(deltaTime, 0f);

            // Set our platform's goal pose to the animation's
            goalPosition = transformCached.position;
            goalRotation = transformCached.rotation;

            // Reset the actual transform pose to where it was before evaluating. 
            // This is so that the real movement can be handled by the physics mover; not the animation
            transformCached.position = _positionBeforeAnim;
            transformCached.rotation = _rotationBeforeAnim;
        }
    }
}