using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class EasyObby_AI : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _aiPrefab;
        [SerializeField] private int _count = 5;
        [SerializeField] private Vector2 _idleDurationRange = new Vector2(0f, 1f);

        private void Start()
        {
            for (int i = 0; i < _count; i++)
            {
                AI ai = _aiPrefab.Create().GetComponent<AI>();

                ai.character.gameObject.AddComponent<EasyObby_Character>();
                ai.character.gameObject.AddComponent<EasyObby_Character_AI>();

                float idleDuration = _idleDurationRange.RandomWithin();
                ai.SetIdleDurationRange(Vector2.one * idleDuration);
            }
        }
    }
}
