using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_TargetRandom : IEvent
    {
        public Transform transform { get; private set; }
        public GameObject explosion { get; private set; }

        public Event_BladeBall_TargetRandom(Transform transform, GameObject explosion)
        {
            this.transform = transform;
            this.explosion = explosion;
        }
    }
}
