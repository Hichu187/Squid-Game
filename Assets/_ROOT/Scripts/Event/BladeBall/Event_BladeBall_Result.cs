using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Event_BladeBall_Result : IEvent
    {
        public bool isWin { get; private set; }

        public Event_BladeBall_Result(bool isWin)
        {
            this.isWin = isWin;
        }
    }
}
