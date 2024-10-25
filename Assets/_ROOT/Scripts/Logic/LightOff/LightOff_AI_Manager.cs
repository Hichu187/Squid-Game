using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LightOff_AI_Manager : MonoBehaviour
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
        }

        private void OnDestroy()
        {
           
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

                ai.gameObject.AddComponent<AIFollowWaypoint>();

                Vector3 position = Random.insideUnitSphere * _randomPositionRadius;

                position.y = 0f;

                ai.character.Revive(position, Quaternion.LookRotation(Vector3.forward, Vector3.up));

                Character character = ai.transform.GetChild(0).GetComponent<Character>();

                character.gameObject.AddComponent<LightOff_AI>().Construct(i);

                float idleDuration = _idleDurationRange.RandomWithin();

                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));

                _ai[i] = ai;
            }
        }
    }

    public enum LightOff_AIType {Normal, Random, Target}
}
