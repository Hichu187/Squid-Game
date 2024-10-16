using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TugOfWar_Player : MonoBehaviour
    {
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();

            _player.gui.gameObject.SetActive(false);
            _player.character.motor.enabled = false;
        }
    }
}
