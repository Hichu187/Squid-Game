using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MarbleShooting_Count : MonoBehaviour
    {
        public List<Image> playerScore;
        public List<Image> otherScore;

        public void ResetScore()
        {
            foreach (var item in playerScore)
            {
                item.color = Color.white;
            }

            foreach (var item in otherScore)
            {
                item.color = Color.white;
            }
        }
    }
}
