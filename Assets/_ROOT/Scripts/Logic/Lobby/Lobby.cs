using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Lobby : MonoBehaviour
    {
        private Lobby_Master _master;

        [Title("Reference")]
        [SerializeField] private float _prepareTime;
        [SerializeField] Transform _piggyPos;
        private void Awake()
        {
            StaticBus<Event_Lobby_Constructed>.Subscribe(Constructed);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Lobby_Constructed>.Subscribe(Constructed);
        }

        private void Start()
        {
            _master = GetComponent<Lobby_Master>();
        }

        void Constructed(Event_Lobby_Constructed e)
        {
            _master.player.cameraManager.cameraTutorial.Play(_piggyPos);

            StartCoroutine(PrepareStart());
        }

        IEnumerator PrepareStart()
        {
            yield return new WaitForSeconds(5);

            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _master.gui.announcement.PushMesseage($"The game will be selected in {Mathf.CeilToInt(currentTime)} second").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }

            _master.gui.announcement.PushMesseage($"Game start !!!").Forget();

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(2);
        }
    }
}
