using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ColorBlock_AI_Manager : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _count = 5;
        [SerializeField] private Vector2 _idleDurationRange = new Vector2(0f, 0.5f);
        [SerializeField] private float _randomPositionRadius;

        private AI[] _ai;

        private void Start()
        {
            SpawnAI();
        }

        private void SpawnAI()
        {
            _ai = new AI[_count];

            for (int i = 0; i < _count; i++)
            {
                AI ai = _prefab.Create().GetComponent<AI>();

                Vector3 position = Random.insideUnitSphere * _randomPositionRadius;
                position.y = 0f;

                ai.character.Revive(position, Quaternion.LookRotation(position.normalized, Vector3.up));

                ai.gameObject.AddComponent<ColorBlock_AI_Character>().Construct(i * 2);

                float idleDuration = _idleDurationRange.RandomWithin();

                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));

                _ai[i] = ai;
            }
        }
    }
}
