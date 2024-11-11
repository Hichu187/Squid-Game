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
        [SerializeField] RuntimeAnimatorController baseAnimator;

        private Weapon curType;

        private void Start()
        {
            _player = transform.parent.GetComponent<Player>();
            curHp = hp;
            healthbar.InitHealthBar(curHp);

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)){
                _player.character.animator._animator.SetTrigger("Attack");
            }
        }

        public void SetAnimator(RuntimeAnimatorController _animator, int id, Weapon type)
        {
            if(weapon == null)
            {
                weapon = transform.GetComponentInChildren<LightOff_HandWeapon>();
            }

            _player.character.animator.SetAnimator(_animator);

            _player.character.animator._animator.SetFloat("Weapon", (int)type);
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
                case Weapon.Slash:
                    f.radius = 1.5f;
                    f.angle = 120f;
                    break;
                case Weapon.Spin:
                    f.radius = 1.5f;
                    f.angle = 360;
                    break;
                case Weapon.Stab:
                    f.radius = 3;
                    f.angle = 45f;
                    break;
                case Weapon.Smash:
                    f.radius = 2.5f;
                    f.angle = 120f;
                    break;
            }
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
        public void Win() 
        {
            _player.character.animator.SetAnimator(baseAnimator);
        }

    }
    public enum Weapon {Empty, Slash, Spin, Stab, Smash}
}
