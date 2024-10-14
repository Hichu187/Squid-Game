using LFramework;
using System;
using UnityEngine;

namespace Game
{
    public class Teleporter : MonoCached
    {
        [SerializeField] Teleporter _otherTeleporter;

        public event Action<CharacterKC> eventCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBeingTeleportedTo)
            {
                CharacterKC ckc = other.GetComponent<CharacterKC>();

                if (ckc)
                {
                    ckc.motor.SetPositionAndRotation(_otherTeleporter.transformCached.position, _otherTeleporter.transformCached.rotation);

                    eventCharacterTeleport?.Invoke(ckc);

                    _otherTeleporter.isBeingTeleportedTo = true;
                }
            }

            isBeingTeleportedTo = false;
        }
    }
}