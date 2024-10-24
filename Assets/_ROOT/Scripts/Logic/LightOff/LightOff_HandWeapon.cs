using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_HandWeapon : MonoBehaviour
    {
        public List<GameObject> weapon;

        public void SetWeapon(int i)
        {
            foreach (GameObject weapon in weapon)
            {
                weapon.SetActive(false);
            }
            weapon[i].SetActive(true);
        }
    }
}
