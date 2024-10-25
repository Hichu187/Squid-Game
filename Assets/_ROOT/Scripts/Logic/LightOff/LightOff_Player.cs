using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_Player : MonoBehaviour
    {
        private Player _player;
        public int weaponId;

        int hp = 5;
        [SerializeField] int dmg = 1;
        [SerializeField] private int curHp;
        [SerializeField] UIHealthbar healthbar;
        [SerializeField] private LightOff_HandWeapon weapon;
        [SerializeField] private FieldOfView fov;

        private Weapon curType;

        private void Start()
        {
            _player = transform.parent.GetComponent<Player>();
            curHp = hp;
            healthbar.InitHealthBar(curHp);

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
                    f.angle = 45f;
                    break;
            }
        }

        public void Attack()
        {
            _player.character.animator.PlayBlock();

        }

        public void DealDamage()
        {
            foreach (var ai in fov.visibleTargets)
            {
                ai.GetComponent<LightOff_AI>().TakeDamage(dmg);
            }
        }
        public void TakeDamage(int damage)
        {
            curHp -= damage;

            if (!healthbar.gameObject.activeSelf)
            {
                healthbar.gameObject.SetActive(true);
            }

            healthbar.UpdateHealthBar(curHp);

            if (curHp <= 0)
            {
                _player.character.Kill();

                healthbar.gameObject.SetActive(false);
            }
        }
    }
    public enum Weapon {Empty, ConeRange, CircleRange, ForwardRange}
}
