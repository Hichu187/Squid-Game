using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_AI : MonoBehaviour
    {
        public int id;
        [SerializeField] private int _surviveCount;
        [SerializeField] private bool _isStart;
        [SerializeField] private bool _isSurvive;
        private AI _ai;
        [SerializeField] private GameObject _aim;
        private bool _goal;

        [SerializeField] int _step = 0;
        int hp = 5;
        [SerializeField] int dmg = 1;
        [SerializeField] private int curHp;
        private void Awake()
        {
            _ai = GetComponent<AI>();
            _isSurvive = true;
            
            curHp = hp;
        }

        private void OnDestroy()
        {
        }

        private async void Start()
        {
            _ai.eventChaseComplete += AI_EventChaseComplete;
            _ai.eventIdleComplete += AI_EventIdleComplete;

            await UniTask.WaitForSeconds(Random.Range(0.5f, 1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            //_ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 1)));

        }

        private void AI_EventIdleComplete()
        {
            //_ai.Chase(_ai.character.transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-1, 1)));
        }

        private void AI_EventChaseComplete()
        {
            _ai.Idle();
        }

        public void TakeDamage(int damage)
        {
            curHp -= damage;

            if(curHp <= 0)
            {     
                _ai.character.Kill();
                _ai.character.gameObject.layer = 6;
            }
        }

        public void Construct(int i)
        {
            _surviveCount = i;

            _aim = _ai.character.transform.GetChild(0).GetChild(0).gameObject;
        }
    }
}
