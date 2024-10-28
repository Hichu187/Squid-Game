using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_Gameplay : MonoBehaviour
    {
        private LightOff_Master _master;

        [Title("Reference")]
        [SerializeField] LightOff_Player _player;
        [SerializeField] private float _gameTime;
        [SerializeField] private float _prepareTime;
        [SerializeField] GameObject _weaponBase;
        [SerializeField] Light roomLight;

        [SerializeField] float blackoutDuration = 5f;
        [SerializeField] float blackoutInterval = 7f;

        private void Awake()
        {
            StaticBus<Event_LightOff_Constructed>.Subscribe(Constructed);
            StaticBus<Event_Player_Die>.Subscribe(Lose);
        }

        private void OnDestroy()
        {
            StaticBus<Event_LightOff_Constructed>.Unsubscribe(Constructed);
            StaticBus<Event_Player_Die>.Unsubscribe(Lose);
        }
        void Start()
        {
            _master = GetComponent<LightOff_Master>();
        }
        void Constructed(Event_LightOff_Constructed e)
        {
            StartCoroutine(PrepareStart());
        }
        IEnumerator PrepareStart()
        {
            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _master.gui.announcement.PushMesseage($"Game start in {Mathf.CeilToInt(currentTime)} second").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }
            _master.gui.announcement.PushMesseage($"Game start !!!").Forget();

            StaticBus<Event_LightOff_Start>.Post(null);

            _weaponBase.gameObject.SetActive(true);

            StartCoroutine(GameTimeCountDown());
            StartCoroutine(BlackoutRoutine());
        }
        IEnumerator GameTimeCountDown()
        {
            float currentTime = _gameTime;

            while (currentTime > 0)
            {
                _master.gui.gameTime.PushMesseage($"{Mathf.CeilToInt(currentTime)}").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }
            _master.gui.gameTime.PushMesseage($"Finish !!!").Forget();
            StaticBus<Event_LightOff_Win>.Post(null);
            _master.SpawnResultView().Forget();

            _player.Win();
        }
        private IEnumerator BlackoutRoutine()
        {
            while (true)
            {
                roomLight.enabled = false;
                yield return new WaitForSeconds(blackoutDuration);

                roomLight.enabled = true;
                yield return new WaitForSeconds(blackoutInterval);
            }
        }

        void Lose(Event_Player_Die e)
        {
            _master.SpawnResultLose().Forget();
            StopAllCoroutines();
        }

    }
}
