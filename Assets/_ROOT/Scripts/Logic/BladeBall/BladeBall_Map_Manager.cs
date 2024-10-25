using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BladeBall_Map_Manager : MonoBehaviour
    {
        [SerializeField] Transform _pStartPosition;

        [SerializeField] Player _player;

        private void Start()
        {
            _player.transform.position = _pStartPosition.position;
        }

    }
}
