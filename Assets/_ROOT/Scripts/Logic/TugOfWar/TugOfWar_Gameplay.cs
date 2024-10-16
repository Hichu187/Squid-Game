using LFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace Game
{
    public class TugOfWar_Gameplay : MonoBehaviour
    {
        private TugOfWar_Master _master;

        [Title("Reference")]
        [SerializeField] bool _isTugOfWar;
        [SerializeField] RectTransform _target;
        [SerializeField] RectTransform _hook;
        [SerializeField] Image _progress;
        [SerializeField] private float _prepareTime;
        [Title("Config")]

        [SerializeField] private float timeMultiplicator = 3;
        [SerializeField] private float smoothMotion = 3;
        [SerializeField] private float _maxSpeed = 3;
        [SerializeField] private float _pullSpeed = 75;
        [SerializeField] float fillSpeed = 0.5f;

        private float _position;
        private float _destination;
        private float timer;
        private float _speed;
        private float _velocity = 0f;

        private bool _pull;
        private Vector2 currentPosition;
        private Vector3 playerPosition;


        private void Awake()
        {
            StaticBus<Event_TurOfWar_Constructed>.Subscribe(StartFight);
            StaticBus<Event_Player_Die>.Subscribe(Lose);

        }

        private void OnDestroy()
        {
            StaticBus<Event_TurOfWar_Constructed>.Unsubscribe(StartFight);
            StaticBus<Event_Player_Die>.Unsubscribe(Lose);
        }

        private void Start()
        {
            _master = GetComponent<TugOfWar_Master>();
        }

        private void Update()
        {
            if(_hook ==  null) return;
            if (!_isTugOfWar) return;
            TargetMoving();
            Pull();
            Progress();
        }
        private void Init()
        {
            UIPointerClick action = _master.gui.pull.GetComponent<UIPointerClick>();
            action.eventDown += () => { _pull = true; };
            action.eventUp += () => { _pull = false; };

            _target = _master.gui.target;
            _hook = _master.gui.hook;
            currentPosition = _hook.anchoredPosition;
            _progress = _master.gui.progress;
        }

        public void StartFight(Event_TurOfWar_Constructed e)
        {
            Init();
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

            StaticBus<Event_RedLightGreenLight_GameStart>.Post(null);
            _isTugOfWar = true;
        }
        void TargetMoving()
        {
            timer -= Time.deltaTime;

            if (timer < 0f)
            {
                timer = UnityEngine.Random.value * timeMultiplicator;

                _destination = UnityEngine.Random.value;
            }

            _position = Mathf.SmoothDamp(_position, _destination, ref _velocity, smoothMotion, _maxSpeed);

            _target.anchoredPosition = Vector2.Lerp(Vector2.down * 175, Vector2.up * 175, _position);
        }
        void Pull()
        {
            if (_pull)
            {
                currentPosition.y += _pullSpeed * Time.deltaTime;
                if (currentPosition.y > 175)
                {
                    currentPosition.y = 175;
                }
            }
            else
            {
                currentPosition.y -= _pullSpeed * 1.5f * Time.deltaTime;
                if (currentPosition.y < -175)
                {
                    currentPosition.y = -175;
                }
            }

            _hook.anchoredPosition = currentPosition;
        }
        void Progress()
        {
            float distance = Vector2.Distance(_hook.anchoredPosition, _target.anchoredPosition);

            playerPosition = _master.player.character.transform.position;

            if (distance < 45)
            {
                _progress.fillAmount += fillSpeed * Time.deltaTime;

                if (_progress.fillAmount == 1) Win();
            }

            else
            {
                _progress.fillAmount -= fillSpeed *1.5f * Time.deltaTime;

                if (_progress.fillAmount <= 0) SetFalling();
            }

            _progress.fillAmount = Mathf.Clamp(_progress.fillAmount, 0f, 1f);

            float newZPosition = Mathf.Lerp(10, -10, _progress.fillAmount);

            playerPosition.z = newZPosition;

            _master.player.character.transform.position = playerPosition;
            _master.player.character.animator.SetVelocityZ(0.3f);
        }

        void Win()
        {
            _isTugOfWar = false;
            _master.player.character.animator.SetVelocityZ(0);
            _master.player.character.animator.PlayWin();
            _master.SpawnResultView().Forget();
        }

        void SetFalling()
        {
            _isTugOfWar = false;
            _master.player.character.animator.SetJumping(true);
        }

        void Lose(Event_Player_Die e)
        {
            _master.SpawnResultLose().Forget();
        }
    }
}
