using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_WeaponSpawn : MonoBehaviour
    {
        public LightOff_Weapon curWeapon;
        [SerializeField] int id;
        [SerializeField] RuntimeAnimatorController _ac;

        [SerializeField] List<GameObject> weapon;
        [SerializeField] Weapon weaponType;

        private void Start()
        {
            foreach (var w in weapon)
            {
                w.gameObject.SetActive(false);
            }

            weapon[id].SetActive(true);
            curWeapon = weapon[id].GetComponent<LightOff_Weapon>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.transform.parent.GetComponent<LightOff_Player>())
            {
                LightOff_Player p = other.transform.parent.GetComponent<LightOff_Player>();

                int tmp = p.weaponId;

                p.SetAnimator(_ac, id, curWeapon.type);

                if (tmp == -1)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    id = tmp;

                    foreach (var w in weapon)
                    {
                        w.gameObject.SetActive(false);
                    }

                    weapon[id].SetActive(true);

                    curWeapon = weapon[id].GetComponent<LightOff_Weapon>();
                }


            }
        }
    }
}
