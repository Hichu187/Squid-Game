using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_Player : MonoBehaviour
    {
        private Player _player;
        public int weaponId;


        private void Start()
        {
            _player = GetComponent<Player>();
        }
        public void SetAnimator(RuntimeAnimatorController _animator, int id)
        {
            _player.character.animator.SetAnimator(_animator);
            weaponId = id;
        }
    }
    public enum Weapon {Empty, ConeRange, CircleRange, ForwardRange}
}
