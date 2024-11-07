using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Segment : MonoBehaviour
    {
        public List<GameObject> parts;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tool"))
            {
                if(parts.Count > 0)
                {
                    foreach (GameObject part in parts)
                    {
                        part.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                transform.parent.GetComponent<Dalgona_Candy>().IncreaseProgress();
                this.gameObject.SetActive(false);

            }
        }
    }
}
