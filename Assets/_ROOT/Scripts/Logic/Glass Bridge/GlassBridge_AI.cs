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
        [SerializeField] private bool _isSurvive;
        private AI _ai;
        [SerializeField] private GameObject _aim;
        private bool _goal;

        [SerializeField] int _step= 0;
        private void Awake()
        {
            _ai = GetComponent<AI>();
            _gameplay = FindObjectOfType<GlassBridge_Gameplay>();

        }

        private void OnDestroy()
        {
        }

        private async void Start()
        {
            await UniTask.WaitForSeconds(Random.Range(0.5f, 1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            _ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 2)));

            _isSurvive = true;
        }
        public IEnumerator JumpRoutine()
        {
            while (true)
            {
                if (_isSurvive)
                {
                    _surviveCount--;

                    if (_step < _gameplay.steps.Count) _ai.Chase(_gameplay.steps[_step].transform);

                    _step++;
                    if (_surviveCount <= 0) { _isSurvive = false; }
                }
                else
                {
                    if (_step < _gameplay.breaks.Count) _ai.Chase(_gameplay.breaks[_step].transform);
                }

                float waitTime = Random.Range(1f, 4f);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public void Jump()
        {
            StartCoroutine(JumpRoutine());
        }

        IEnumerator Dead()
        {
            StopAllCoroutines();
            yield return new WaitForSeconds(1.5f);
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
