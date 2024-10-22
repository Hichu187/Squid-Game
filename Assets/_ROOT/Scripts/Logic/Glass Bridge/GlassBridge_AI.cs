using Cysharp.Threading.Tasks;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_AI : MonoBehaviour
    {
        public int id;
        private GlassBridge_Gameplay _gameplay;
        [SerializeField] private int _surviveCount;
        [SerializeField] private bool _isStart;
        [SerializeField] private bool _isSurvive;
        private AI _ai;
        [SerializeField] private GameObject _aim;
        private bool _goal;

        [SerializeField] int _step= 0;
        private void Awake()
        {
            _ai = GetComponent<AI>();
            _gameplay = FindObjectOfType<GlassBridge_Gameplay>();
            _ai.character.GetComponent<CharacterFallDetector>().enabled = false;
            _isSurvive = true;
        }

        private void OnDestroy()
        {
        }

        private async void Start()
        {
            _ai.eventChaseComplete += AI_EventChaseComplete;
            _ai.eventIdleComplete += AI_EventIdleComplete;

            await UniTask.WaitForSeconds(Random.Range(0.5f, 1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            _ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 1)));

        }

        private void AI_EventIdleComplete()
        {
            if (!_isStart)
            {
                _ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-1, 1)));
            }
        }

        private void AI_EventChaseComplete()
        {
            _ai.Idle();
        }
        public IEnumerator JumpRoutine()
        {
            while (true)
            {
                if (_step >= _gameplay.steps.Count)
                {
                    if (!_isSurvive) break;
                    else { _ai.Chase(_gameplay.stopPosition.transform.GetChild(1)); break; }
                }

                var currentBreak = _gameplay.breaks[_step];
                var currentStep = _gameplay.steps[_step];

                if (!currentBreak.isBreak)
                {
                    float r = Random.value;
                    if (r >= 0.5f)
                    {
                        _ai.Chase(currentStep.transform);
                    }
                    else
                    {
                        _ai.Chase(currentBreak.transform);
                        StartCoroutine(Dead());

                    }
                }
                else
                {
                    _ai.Chase(currentStep.transform);
                }
                _step++;

                float waitTime = Random.Range(1f, 4f);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public void Jump()
        {
            if (!_isStart)
            {
                _isStart = true;
            }

            StartCoroutine(JumpRoutine());
        }

        IEnumerator Dead()
        {
            _isSurvive = false;
            yield return new WaitForSeconds(2f);
            _aim.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);
            _ai.character.Kill();
            _aim.gameObject.SetActive(false);

        }

        public void Construct(int i)
        {
            _surviveCount = i;

            _aim = _ai.character.transform.GetChild(0).GetChild(0).gameObject;
        }
    }
}
