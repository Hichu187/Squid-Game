using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BladeBall_DummyTest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BladeBall_Ball"))
            {
                StaticBus<Event_BladeBall_TargetPlayer>.Post(null);
            }
        }
    }
}
