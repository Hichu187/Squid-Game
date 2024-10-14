using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryTowerOfHell : ScriptableObjectSingleton<FactoryTowerOfHell>
    {
        [ReadOnly]
        [SerializeField] private int _checkpointCount;

        public static int checkpointCount { get => instance._checkpointCount; set => instance._checkpointCount = value; }
    }
}
