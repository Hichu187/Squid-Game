using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_WeaponSpawn : MonoBehaviour
    {
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
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.transform.parent.GetComponent<LightOff_Player>())
            {
                LightOff_Player p = other.transform.parent.GetComponent<LightOff_Player>();

                int tmp = p.weaponId;

                p.SetAnimator(_ac, id);

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
                }


            }
        }
    }
}
