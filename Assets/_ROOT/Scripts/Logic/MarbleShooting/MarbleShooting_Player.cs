using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MarbleShooting_Player : MonoBehaviour
    {
        [SerializeField] GameObject _target;
        public bool isTarget;

        public void GetTarget()
        {
            _target.SetActive(true);
            StartCoroutine(Dead());
        }

        IEnumerator Dead()
        {
            yield return new WaitForSeconds(1.5f);

            GetComponent<Player>().character.Kill();
            _target.SetActive(false);
        }
    }
}
