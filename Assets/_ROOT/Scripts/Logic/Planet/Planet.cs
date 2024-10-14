using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;

namespace Game
{
    public class Planet : MonoBehaviour, IMoverController
    {
        [Title("Reference")]
        [SerializeField] private PhysicsMover _mover;
        [SerializeField] private SphereCollider _gravityField;

        [Space]

        [SerializeField] private Teleporter _teleporterIn;
        [SerializeField] private Teleporter _teleporterOut;

        [Title("Config")]
        [SerializeField] private float _gravityStrength = 10;
        [SerializeField] private Vector3 _orbitAxis = Vector3.forward;
        [SerializeField] float _orbitSpeed = 10;

        private List<CharacterKC> _characterControllersOnPlanet = new List<CharacterKC>();
        private Vector3 _savedGravity;
        private Quaternion _lastRotation;

        private void OnEnable()
        {
            _teleporterIn.eventCharacterTeleport += ControlGravity;
            _teleporterOut.eventCharacterTeleport += UnControlGravity;
        }

        private void OnDisable()
        {
            _teleporterIn.eventCharacterTeleport -= ControlGravity;
            _teleporterOut.eventCharacterTeleport -= UnControlGravity;
        }

        private void Start()
        {
            _lastRotation = _mover.transform.rotation;

            _mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = _mover.Rigidbody.position;

            // Rotate
            Quaternion targetRotation = Quaternion.Euler(_orbitAxis * _orbitSpeed * deltaTime) * _lastRotation;
            goalRotation = targetRotation;
            _lastRotation = targetRotation;

            // Apply gravity to characters
            foreach (CharacterKC cc in _characterControllersOnPlanet)
            {
                cc.gravity = (_mover.transform.position - cc.transform.position).normalized * _gravityStrength;
            }
        }

        void ControlGravity(CharacterKC ckc)
        {
            _savedGravity = ckc.gravity;
            _characterControllersOnPlanet.Add(ckc);
        }

        void UnControlGravity(CharacterKC ckc)
        {
            ckc.gravity = _savedGravity;
            _characterControllersOnPlanet.Remove(ckc);
        }
    }
}