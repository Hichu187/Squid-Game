using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_Raiser : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private Transform _platformLava;

        private float _raiseSpeed;
        private bool _raiseEnable = false;

        public event Action<float> eventTimeRemain;
        public event Action eventComplete;

        private void Awake()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Subscribe(StaticBus_TheFloorIsLavaLevelLoaded);
            StaticBus<Event_Player_Die>.Subscribe(StaticBus_Player_Die);
        }

        private void OnDestroy()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Unsubscribe(StaticBus_TheFloorIsLavaLevelLoaded);
            StaticBus<Event_Player_Die>.Unsubscribe(StaticBus_Player_Die);
        }

        private void Start()
        {
            _platformLava.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_raiseEnable)
                return;

            _platformLava.TranslateY(_raiseSpeed * Time.deltaTime);

            eventTimeRemain?.Invoke(((TheFloorIsLava_Static.level.lavaHeight - _platformLava.position.y) / TheFloorIsLava_Static.level.lavaHeight) * TheFloorIsLava_Static.level.lavaDuration);

            if (_platformLava.position.y >= TheFloorIsLava_Static.level.lavaHeight)
            {
                _platformLava.SetY(TheFloorIsLava_Static.level.lavaHeight);

                _raiseEnable = false;

                eventComplete?.Invoke();
            }
        }

        private void StaticBus_TheFloorIsLavaLevelLoaded(Event_TheFloorIsLava_LevelConstructed e)
        {
            // Calculate raise speed
            _raiseSpeed = TheFloorIsLava_Static.level.lavaHeight / TheFloorIsLava_Static.level.lavaDuration;

            // Setup platform lava
            _platformLava.SetScale(TheFloorIsLava_Static.level.bounds.size.x * 0.1f, 1.0f, TheFloorIsLava_Static.level.bounds.size.z * 0.1f);
            _platformLava.position = new Vector3(TheFloorIsLava_Static.level.bounds.center.x, TheFloorIsLava_Static.level.bounds.min.y, TheFloorIsLava_Static.level.bounds.center.z);

            // Hide platform lava
            _platformLava.gameObject.SetActive(false);
        }

        private void StaticBus_Player_Die(Event_Player_Die e)
        {
            _raiseEnable = false;
        }

        public void Raise(float deltaY)
        {
            _platformLava.TranslateY(deltaY);
            _platformLava.gameObject.SetActive(true);

            _raiseEnable = true;
        }
    }
}
