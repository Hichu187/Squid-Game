using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_AiDie : IEvent
    {
        public Character character { get; private set; }

        public Event_BladeBall_AiDie(Character character)
        {
            this.character = character;
        }
    }
}
