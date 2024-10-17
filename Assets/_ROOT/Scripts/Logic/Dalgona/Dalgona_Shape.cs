using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Shape : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Tool")
            {
                Debug.Log("Crack");
            }
        }
    }
}
