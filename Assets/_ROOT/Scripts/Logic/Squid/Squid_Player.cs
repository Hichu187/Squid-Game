using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Squid_Player : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private LightOff_HandWeapon _weapon;
        [SerializeField] FieldOfView fov;

        int hp = 5;
        [SerializeField] int dmg = 1;
        [SerializeField] private int curHp;
        [SerializeField] UIHealthbar healthbar;
        private void Awake()
        {
            StaticBus<Event_Squid_ChooseWeapon>.Subscribe(InitWeapon);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Squid_ChooseWeapon>.Subscribe(InitWeapon);
        }

        private void Start()
        {
            _player = transform.parent.GetComponent<Player>();
            curHp = hp;
            healthbar.InitHealthBar(curHp);
        }

        public void InitWeapon(Event_Squid_ChooseWeapon e)
        {
            _weapon = transform.GetComponentInChildren<LightOff_HandWeapon>();
            _weapon.SetWeapon(e.id);
        }

        public void DealDamage()
        {
            foreach (var ai in fov.visibleTargets)
            {
                //ai.GetComponent<Squid_AI>().TakeDamage(dmg);
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
}
