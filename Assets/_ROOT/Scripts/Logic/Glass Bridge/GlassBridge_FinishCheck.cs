using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_FinishCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if(other.transform.parent.GetComponent<GlassBridge_Player>().isComplete == false)
                {
                    other.transform.parent.GetComponent<GlassBridge_Player>().isComplete = true;
                }
            }
        }
    }
}
