using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_LevelPoints : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _highestPoint;
        [SerializeField] private Transform _cameraPoint;

        public Transform spawnPoint { get { return _spawnPoint; } }
        public Transform highestPoint { get { return _highestPoint; } }
        public Transform cameraPoint { get { return _cameraPoint; } }
    }
}
