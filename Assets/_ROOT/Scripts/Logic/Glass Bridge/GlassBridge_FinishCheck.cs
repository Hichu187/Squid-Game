using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_FinishCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.transform.parent.GetComponent<GlassBridge_Player>())
            {
                other.transform.parent.GetComponent<GlassBridge_Player>().PlayerGoal();
            }
        }
    }
}
