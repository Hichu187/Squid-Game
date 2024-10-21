using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class GlassBridge_Gameplay : MonoBehaviour
    {
        private GlassBridge_Master _master;

        [Title("Reference")]
        [SerializeField] private float _gameTime;
        [SerializeField] private float _prepareTime;
        [SerializeField] private int _step;
        [SerializeField] Transform _startPosition;
        [SerializeField] Transform _stopPosition;
        [SerializeField] Transform _wall;

        [SerializeField] private GameObject _stepPrefab;
        [SerializeField] Transform _line1;
        [SerializeField] Transform _line2;

        [SerializeField] List<GameObject> line1Step;
        [SerializeField] List<GameObject> line2Step;
        [SerializeField] List<BridgePart> steps;

        private void Awake()
        {
            StaticBus<Event_GlassBridge_Constructed>.Subscribe(Constructed);
            StaticBus<Event_Player_Die>.Subscribe(Lose);
        }

        private void OnDestroy()
        {
            StaticBus<Event_GlassBridge_Constructed>.Unsubscribe(Constructed);
            StaticBus<Event_Player_Die>.Unsubscribe(Lose);
        }

        private void Start()
        {
            _master = GetComponent<GlassBridge_Master>();
            _master.player.character.motor.SetPosition(_startPosition.position);
        }

        private void Init()
        {            
            for(int i = 0; i < _step; i++)
            {
                GameObject step = Instantiate(_stepPrefab, _line1);
                step.transform.localPosition = new Vector3(0, 0, 4 * i);
                GameObject step2 = Instantiate(_stepPrefab, _line2);
                step2.transform.localPosition = new Vector3(0, 0, 4 * i);

                line1Step.Add(step);
                line2Step.Add(step2);

                if (Random.value > 0.5f)
                {
                    steps.Add(line1Step[i].GetComponent<BridgePart>());
                }
                else
                {
                    steps.Add(line2Step[i].GetComponent<BridgePart>());
                }
            }

            _stopPosition.position = new Vector3(0, 0, steps[steps.Count - 1].transform.position.z + 5.45f);

            if(steps != null)
            {
                foreach(var step in steps)
                {
                    step.SetCollider();
                }
            }
        }
        public void Constructed(Event_GlassBridge_Constructed e)
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

            StaticBus<Event_GlassBridge_Start>.Post(null);

            _wall.gameObject.SetActive(false);

            StartCoroutine(Hint());
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

            foreach(var step in line1Step)
            {
                step.GetComponent<BridgePart>().Break();
            }
            foreach (var step in line2Step)
            {
                step.GetComponent<BridgePart>().Break();
            }

            if (_master.player.GetComponent<GlassBridge_Player>().isComplete)
            {
                _master.SpawnResultView().Forget();
            }
            else
            {
                _master.SpawnResultLose().Forget();
            }
        }

        void Lose(Event_Player_Die e)
        {
            _master.SpawnResultLose().Forget();
            StopAllCoroutines();
        }
        IEnumerator Hint()
        {
            while (true)
            {
                yield return new WaitForSeconds(4.75f);

                if (steps.Count > 0)
                {
                    int randomIndex = Random.Range(0, steps.Count);

                    steps[randomIndex].Hint();
                }
            }
        }
    }
}
