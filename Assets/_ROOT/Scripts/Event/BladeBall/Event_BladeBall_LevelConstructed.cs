using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_LevelConstructed : IEvent
    {
        public List<AIType> ais {  get; private set; }

        public Event_BladeBall_LevelConstructed(List<AIType> ais)
        {

        this.ais = ais; 
        }
    }
}
