using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Lobby_Door : MonoBehaviour
    {
        public Lobby lobby;
        bool isStart = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponentInParent<Player>() && !isStart)
            {
                isStart = true; 
                lobby.LoadGameScene();
            }
        }
    }
}
