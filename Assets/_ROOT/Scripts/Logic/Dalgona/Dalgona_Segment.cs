using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Segment : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tool") && GetComponent<Renderer>().material.color != Color.green)
            {
                GetComponent<Renderer>().material.color = Color.green;
                transform.parent.GetComponent<Dalgona_Candy>().IncreaseProgress();
            }
        }
    }
}
