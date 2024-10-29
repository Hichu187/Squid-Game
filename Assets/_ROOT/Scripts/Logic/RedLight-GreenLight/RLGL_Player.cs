using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RLGL_Player : MonoBehaviour
    {
        [SerializeField] GameObject _target;
        public bool isTarget;

        public bool isCompleted = false;
        public void GetTarget()
        {
            _target.SetActive(true);
            StartCoroutine(Dead());
        }

        IEnumerator Dead()
        {
            yield return new WaitForSeconds(1.5f);

            GetComponent<Character>().Kill();
            _target.SetActive(false);

            StaticBus<Event_Player_Die>.Post(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Goal"))
            {
                isCompleted = true;
            }
        }
    }
}
