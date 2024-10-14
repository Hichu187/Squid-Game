/*
using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game
{
    public class CameraThirdPerson_Legacy : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;

        [Title("Follow")]
        [SerializeField] private Vector2 _followPointFraming = new Vector2(0f, 0f);
        [SerializeField] private float _followSharpness = 10000f;

        [Title("Distance")]
        [SerializeField] private float _distance = 6f;
        [SerializeField] private float _distanceMin = 0f;
        [SerializeField] private float _distanceMax = 10f;
        [SerializeField] private float _distanceMovementSpeed = 5f;
        [SerializeField] private float _distanceMovementSharpness = 10f;

        [Title("Rotation")]
        [SerializeField] private bool _invertX = false;
        [SerializeField] private bool _invertY = false;
        [Range(-90f, 90f)]
        [SerializeField] private float _verticalAngle = 20f;
        [Range(-90f, 90f)]
        [SerializeField] private float _verticalAngleMin = -90f;
        [Range(-90f, 90f)]
        [SerializeField] private float _verticalAngleMax = 90f;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _rotationSharpness = 10000f;
        [SerializeField] private bool _rotateWithPhysicsMover = false;

        [Title("Obstruction")]
        [SerializeField] private float _obstructionCheckRadius = 0.2f;
        [SerializeField] private LayerMask _obstructionLayers = -1;
        [SerializeField] private float _obstructionSharpness = 10000f;
        [SerializeField] private Collider[] _ignoredColliders;

        private Vector3 _planarDirection;
        private float _distanceTarget;

        private bool _distanceIsObstructed;
        private float _currentDistance;
        private float _targetVerticalAngle;
        private RaycastHit _obstructionHit;
        private int _obstructionCount;
        private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
        private float _obstructionTime;
        private Vector3 _currentFollowPosition;

        public Collider[] ignoredColliders { get { return _ignoredColliders; } set { _ignoredColliders = value; } }

        public bool rotateWithPhysicsMover { get { return _rotateWithPhysicsMover; } }
        public Vector3 planarDirection { get { return _planarDirection; } set { _planarDirection = value; } }
        public float distanceTarget { get { return _distanceTarget; } set { _distanceTarget = value; } }
        public float distance { get { return _distance; } }

        private const int MaxObstructions = 32;

        void OnValidate()
        {
            _distance = Mathf.Clamp(_distance, _distanceMin, _distanceMax);
            _verticalAngle = Mathf.Clamp(_verticalAngle, _verticalAngleMin, _verticalAngleMax);
        }

        void Awake()
        {
            _currentDistance = _distance;
            _distanceTarget = _currentDistance;

            _targetVerticalAngle = 0f;

            _planarDirection = _target.forward;
            _currentFollowPosition = _target.position;
        }

        public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
        {
            if (!_target)
                return;

            if (_invertX)
                rotationInput.x *= -1f;

            if (_invertY)
                rotationInput.y *= -1f;

            // Process rotation input
            Quaternion rotationFromInput = Quaternion.Euler(_target.up * (rotationInput.x * _rotationSpeed));
            _planarDirection = rotationFromInput * _planarDirection;
            _planarDirection = Vector3.Cross(_target.up, Vector3.Cross(_planarDirection, _target.up));
            Quaternion planarRot = Quaternion.LookRotation(_planarDirection, _target.up);

            _targetVerticalAngle -= (rotationInput.y * _rotationSpeed);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, _verticalAngleMin, _verticalAngleMax);
            Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
            Quaternion targetRotation = Quaternion.Slerp(transformCached.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-_rotationSharpness * deltaTime));

            // Apply rotation
            transformCached.rotation = targetRotation;

            // Process distance input
            if (_distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
            {
                _distanceTarget = _currentDistance;
            }
            _distanceTarget += zoomInput * _distanceMovementSpeed;
            _distanceTarget = Mathf.Clamp(_distanceTarget, _distanceMin, _distanceMax);

            // Find the smoothed follow position
            _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, _target.position, 1f - Mathf.Exp(-_followSharpness * deltaTime));

            // Handle obstructions
            {
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                _obstructionCount = Physics.SphereCastNonAlloc(_currentFollowPosition, _obstructionCheckRadius, -transformCached.forward, _obstructions, _distanceTarget, _obstructionLayers, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < _obstructionCount; i++)
                {
                    bool isIgnored = false;
                    for (int j = 0; j < _ignoredColliders.Length; j++)
                    {
                        if (_ignoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }
                    for (int j = 0; j < _ignoredColliders.Length; j++)
                    {
                        if (_ignoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (!isIgnored && _obstructions[i].distance < closestHit.distance && _obstructions[i].distance > 0)
                    {
                        closestHit = _obstructions[i];
                    }
                }

                // If obstructions detecter
                if (closestHit.distance < Mathf.Infinity)
                {
                    _distanceIsObstructed = true;
                    _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance, 1 - Mathf.Exp(-_obstructionSharpness * deltaTime));
                }
                // If no obstruction
                else
                {
                    _distanceIsObstructed = false;
                    _currentDistance = Mathf.Lerp(_currentDistance, _distanceTarget, 1 - Mathf.Exp(-_distanceMovementSharpness * deltaTime));
                }
            }

            // Find the smoothed camera orbit position
            Vector3 targetPosition = _currentFollowPosition - ((targetRotation * Vector3.forward) * _currentDistance);

            // Handle framing
            targetPosition += transformCached.right * _followPointFraming.x;
            targetPosition += transformCached.up * _followPointFraming.y;

            // Apply position
            transformCached.position = targetPosition;
        }
    }
}
*/