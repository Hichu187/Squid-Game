using Cysharp.Threading.Tasks;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RLGL_AI : MonoBehaviour
    {
        [SerializeField] private int _surviveCount;
        [SerializeField] private bool _isSurvive;
        private AI _ai;
        [SerializeField]  private GameObject _aim;
        private bool _goal;

        private void Awake()
        {
            _ai = GetComponent<AI>();

            StaticBus<Event_RedLightGreenLight_GreenLight>.Subscribe(GreenLight);
            StaticBus<Event_RedLightGreenLight_RedLight>.Subscribe(RedLight);
        }

        private void OnDestroy()
        {
            StaticBus<Event_RedLightGreenLight_GreenLight>.Unsubscribe(GreenLight);
            StaticBus<Event_RedLightGreenLight_RedLight>.Unsubscribe(RedLight);
        }

        private async void Start()
        {
            await UniTask.WaitForSeconds(Random.Range(0.5f, 1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            _ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5,2)));

            _isSurvive = true;
        }

        private void GreenLight(Event_RedLightGreenLight_GreenLight e)
        {
            _ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(17, 30)));
        }

        private void RedLight(Event_RedLightGreenLight_RedLight e)
        {
            if (_goal) return;
            if (_isSurvive)
            {
                _ai.Chase(_ai.character.transform.position);
                _ai.Idle();

                _surviveCount--;
                if (_surviveCount >= 0) _isSurvive = true;
                else _isSurvive = false;
            }
            else
            {
                _aim.gameObject.SetActive(true);
                StartCoroutine(Dead());
            }
        }

        IEnumerator Dead()
        {
            yield return new WaitForSeconds(1.5f);
            _ai.character.Kill();
            _aim.gameObject.SetActive(false);
        }

        public void Construct(int surviveCount)
        {
            _surviveCount = surviveCount;

            _aim = _ai.character.transform.GetChild(0).GetChild(0).gameObject;
        }
    }
}
