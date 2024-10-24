using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_Player : MonoBehaviour
    {
        private Player _player;
        public int weaponId;

        [SerializeField] private LightOff_HandWeapon weapon;
        [SerializeField] private FieldOfView fov;

        private Weapon curType;

        private void Start()
        {
            _player = GetComponent<Player>();
        }
        public void SetAnimator(RuntimeAnimatorController _animator, int id, Weapon type)
        {
            if(weapon == null)
            {
                weapon = transform.GetComponentInChildren<LightOff_HandWeapon>();
            }

            _player.character.animator.SetAnimator(_animator);
            weaponId = id;

            weapon.SetWeapon(id);

            curType = type;
            SetRange();
        }

        public void SetRange()
        {
            FieldOfView f = _player.character.GetComponent<FieldOfView>();

            switch (curType)
            {
                case Weapon.ConeRange:
                    f.radius = 1.5f;
                    f.angle = 120f;
                    break;
                case Weapon.CircleRange:
                    f.radius = 1.5f;
                    f.angle = 360;
                    break;
                case Weapon.ForwardRange:
                    f.radius = 3;
                    f.angle = 30f;
                    break;
            }
        }

        public void Attack()
        {
            _player.character.animator.PlayBlock();

        }

        public void DealDamage()
        {
            Debug.Log(1);
            foreach (var ai in fov.visibleTargets)
            {
                ai.GetComponent<LightOff_AI>().TakeDamage(1);
            }
        }
    }
    public enum Weapon {Empty, ConeRange, CircleRange, ForwardRange}
}
