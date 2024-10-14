using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryEasyObby : ScriptableObjectSingleton<FactoryEasyObby>
    {
        [SerializeField] private float _reviveDelay = 2f;

        [Space]

        [SerializeField] private Vector2Int _aiCheckpointRange = new Vector2Int(-3, 5);

        [Space]

        [SerializeField] private Vector3 _checkpointSize = new Vector3(8f, 1f, 8f);

        [ReadOnly]
        [SerializeField] private int _checkpointCount;

        public static float reviveDelay => instance._reviveDelay;
        public static Vector2Int aiCheckpointRange => instance._aiCheckpointRange;
        public static Vector3 checkpointSize => instance._checkpointSize;
        public static int checkpointCount { get => instance._checkpointCount; set { instance._checkpointCount = value; } }
    }
}
