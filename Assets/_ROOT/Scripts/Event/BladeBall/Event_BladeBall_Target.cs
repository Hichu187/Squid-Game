using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_Target : IEvent
    {
        public Transform transform { get; private set; }
        public GameObject vfx { get; private set; }
        public Event_BladeBall_Target(Transform transform, GameObject vfx)
        {
            this.transform = transform;
            this.vfx = vfx; 
        }
    }
}
