using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TheFloorIsLava_LevelData
    {
        [SerializeField] private bool _isCompleted;

        public bool isCompleted { get { return _isCompleted; } }

        public void Complete()
        {
            _isCompleted = true;
        }
    }
}
