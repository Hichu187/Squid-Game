using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_SelectWeapon : IEvent
    {
        public int id {  get; private set; }
        public Event_BladeBall_SelectWeapon(int id)
        {
            this.id = id;
        }
    
    }
}
