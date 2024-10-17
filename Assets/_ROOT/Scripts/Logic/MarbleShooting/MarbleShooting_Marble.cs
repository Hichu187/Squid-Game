using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MarbleShooting_Marble : MonoBehaviour
    {
        public Rigidbody rb;
        public float stopThreshold = 0.1f;
        public bool hasStopped = false;
        public bool isPlayer;

        void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        void Update()
        {
            if (rb.velocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold )
            {
                if (!hasStopped)
                {
                    StaticBus<Event_MarbleShooting_ChangePhase>.Post(null);
                    hasStopped = true;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tool"))
            {
                StaticBus<Event_MarbleShooting_Count>.Post(new Event_MarbleShooting_Count(isPlayer));
            }
        }
    }
}
