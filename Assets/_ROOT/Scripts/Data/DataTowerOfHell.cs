using LFramework;
using UnityEngine;

namespace Game
{
    public class DataTowerOfHell : LDataBlock<DataTowerOfHell>
    {
        [SerializeField] private Vector3 _playerPosition;
        [SerializeField] private Quaternion _playerRotation;

        [SerializeField] private LValue<int> _checkpointIndex;
        [SerializeField] private bool _checkpointPenalty = false;

        public static Vector3 playerPosition { get { return instance._playerPosition; } set { instance._playerPosition = value; } }
        public static Quaternion playerRotation { get { return instance._playerRotation; } set { instance._playerRotation = value; } }

        public static LValue<int> checkpointIndex { get { return instance._checkpointIndex; } }
        public static bool checkpointPenalty { get { return instance._checkpointPenalty; } set { instance._checkpointPenalty = value; } }

        protected override void Init()
        {
            base.Init();

            _checkpointIndex = _checkpointIndex ?? new LValue<int>(-1);
        }
    }
}
