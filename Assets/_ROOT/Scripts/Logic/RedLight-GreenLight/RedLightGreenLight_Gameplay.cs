using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RedLightGreenLight_Gameplay : MonoBehaviour
    {
        private RedLightGreenLight_Master _master;

        [Title("Reference")]
        [SerializeField] private Transform _target;
        [SerializeField] RLGL_Player _player;

        [Title("Config")]
        [SerializeField] private float _prepareTime;
        [SerializeField] private float _gameTime;
        [SerializeField] GameObject _startLight;

        [SerializeField] bool isGreenLight = true;
        [SerializeField] float greenLightDuration = 5f;
        [SerializeField] float redLightDuration = 3f;

        private Vector3 _playerStopPosition;
        private void Awake()
        {
            StaticBus<Event_RedLightGreenLight_Constructed>.Subscribe(RedLightGreenLightInit);
            StaticBus<Event_Player_Die>.Subscribe(Lose);
        }

        private void OnDestroy()
        {
            StaticBus<Event_RedLightGreenLight_Constructed>.Unsubscribe(RedLightGreenLightInit);
            StaticBus<Event_Player_Die>.Unsubscribe(Lose);
        }
        private void Start()
        {
            _master = GetComponent<RedLightGreenLight_Master>();
        }

        private void Update()
        {
            if (!isGreenLight)
            {
                float dis = Vector3.Distance(_playerStopPosition, _player.transform.position);
                if(dis>0.5f && !_player.isTarget)
                {
                    _player.isTarget = true;
                    _player.GetTarget();
                }
            }
        }

        private void RedLightGreenLightInit(Event_RedLightGreenLight_Constructed e)
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

            _master.gui.notice.gameObject.SetActive(true);

            StaticBus<Event_RedLightGreenLight_GameStart>.Post(null);

            _startLight.GetComponent<Collider>().isTrigger = true;

            StartCoroutine(GameTimeCountDown());
            StartCoroutine(LightSwitching());
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

            ResultCheck();
        }
        IEnumerator LightSwitching()
        {
            while (true)
            {
                isGreenLight = true;

                float count = greenLightDuration + Random.Range(-1, 3);

                StaticBus<Event_RedLightGreenLight_GreenLight>.Post(new Event_RedLightGreenLight_GreenLight(count));

                yield return new WaitForSeconds(count);

                isGreenLight = false;
                _playerStopPosition = _master.player.character.transform.position; 
                StaticBus<Event_RedLightGreenLight_RedLight>.Post(null);
                yield return new WaitForSeconds(redLightDuration);
            }
        }

        void ResultCheck()
        {
            //Player
            if (_player.isCompleted)
            {
                _master.SpawnResultView().Forget();
            }
            else
            {
                _master.player.character.Kill();
            }

            //AI
            RLGL_AI_Manager ais = GetComponent<RLGL_AI_Manager>();
            foreach(var ai in ais._ai)
            {
                ai.Stop();
            }
        }

        void Lose(Event_Player_Die e)
        {
            _master.Lose().Forget();
            StopAllCoroutines();
        }
    }
}
