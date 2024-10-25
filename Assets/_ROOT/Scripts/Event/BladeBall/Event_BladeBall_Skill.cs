using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_Skill : IEvent
    {
        public float cooldown { get; private set; }
        public Event_BladeBall_Skill(float cooldown)
        {
            this.cooldown = cooldown;
        }
    }
}
