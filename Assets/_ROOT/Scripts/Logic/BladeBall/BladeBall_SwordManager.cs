using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class BladeBall_SwordManager : MonoBehaviour
    {
        public List<GameObject> swordModels;
        [SerializeField] List<GameObject> _slashVfx;
        [SerializeField] List<GameObject> _explosionVfx;
        public int currentSwordId;
        private void Awake()
        {
            if(SceneManager.GetActiveScene().name != "MiniGame_BladeBall")
            {
                this.gameObject.SetActive(false);
            }
        }

        public void PlayerSword()
        {
            for (int i = 0; i < swordModels.Count; i++)
            {
                swordModels[i].SetActive(false);
            }
            swordModels[currentSwordId].SetActive(true);
        }
        public void SelectSword()
        {
            currentSwordId = Random.Range(0,swordModels.Count);
            for (int i = 0; i < swordModels.Count; i++)
            {
                swordModels[i].SetActive(false);
            }
            swordModels[currentSwordId].SetActive(true);
        }

        public GameObject SetSlashById()
        {
            return _slashVfx[currentSwordId];
        }

        public GameObject SetExplosionById()
        {
            return _explosionVfx[currentSwordId];
        }
    }
}
