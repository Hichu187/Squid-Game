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
        [SerializeField] private float _gameTime;
        [SerializeField] private float _prepareTime;

        [SerializeField] GameObject _weaponBase;


        private void Awake()
        {
            StaticBus<Event_LightOff_Constructed>.Subscribe(Constructed);

        }

        private void OnDestroy()
        {
            StaticBus<Event_LightOff_Constructed>.Unsubscribe(Constructed);
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

            _master.SpawnResultView().Forget();
        }
        void Lose(Event_Player_Die e)
        {
            _master.SpawnResultLose().Forget();
            StopAllCoroutines();
        }

    }
}
