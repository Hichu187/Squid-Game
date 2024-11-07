using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Candy : MonoBehaviour
    {
        [SerializeField] int hitCount;
        [SerializeField] int progress;
        [SerializeField] List<Dalgona_Segment> segments;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tool"))
            {
                hitCount++;

                // Vibratation

                if(hitCount == 3)
                {
                    this.GetComponent<Renderer>().material.color = Color.red;
                    StaticBus<Event_Dalgona_Lose>.Post(null);
                }
            }
        }

        public void IncreaseProgress()
        {
            progress++;
            if(progress == segments.Count)
            {
                StaticBus<Event_Dalgona_Win>.Post(null);
            }
        }
    }
}
