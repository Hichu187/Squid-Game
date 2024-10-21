using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_AI_Manager : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _prefab;
        public int count = 10;
        [SerializeField] private Vector2 _idleDurationRange = new Vector2(0f, 0.5f);
        [SerializeField] private float _randomPositionRadius;

        private AI[] _ai;

        private void Awake()
        {
            SpawnAI();
            StaticBus<Event_GlassBridge_Start>.Subscribe(RoundStart);
        }

        private void OnDestroy()
        {
            StaticBus<Event_GlassBridge_Start>.Unsubscribe(RoundStart);
        }

        private void Start()
        {

        }


        private void SpawnAI()
        {
            _ai = new AI[count];

            for (int i = 0; i < count; i++)
            {
                AI ai = _prefab.Create().GetComponent<AI>();

                Vector3 position = Random.insideUnitSphere * _randomPositionRadius;
                position.y = 0f;
                position.z = 0f;

                ai.character.Revive(position, Quaternion.LookRotation(Vector3.forward, Vector3.up));

                
                ai.gameObject.AddComponent<GlassBridge_AI>().Construct(i);

                float idleDuration = _idleDurationRange.RandomWithin();

                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));

                _ai[i] = ai;
            }
        }

        private void RoundStart(Event_GlassBridge_Start e)
        {
            StartCoroutine(JumpSequence());
        }

        IEnumerator JumpSequence()
        {
            while (true)
                for (int i = 0; i < _ai.Length; i++)
                {
                    _ai[i].GetComponent<GlassBridge_AI>().Jump();

                    yield return new WaitForSeconds(2.5f);
                }
            
        }
    }
}
