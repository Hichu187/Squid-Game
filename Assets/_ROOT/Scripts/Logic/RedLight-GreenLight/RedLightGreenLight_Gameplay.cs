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
        }

        private void OnDestroy()
        {
            StaticBus<Event_RedLightGreenLight_Constructed>.Unsubscribe(RedLightGreenLightInit);
        }
        private void Start()
        {
            _master = GetComponent<RedLightGreenLight_Master>();
        }

        private void Update()
        {
            if (!isGreenLight)
            {
                float dis = Vector3.Distance(_playerStopPosition, _master.player.character.transform.position);
                if(dis>0.5f && !_master.player.GetComponent<RLGL_Player>().isTarget)
                {
                    _master.player.GetComponent<RLGL_Player>().isTarget = true;
                    _master.player.GetComponent<RLGL_Player>().GetTarget();
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

        void Lose()
        {

        }
    }
}
