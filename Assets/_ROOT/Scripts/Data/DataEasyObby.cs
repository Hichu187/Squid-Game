using LFramework;
using UnityEngine;

namespace Game
{
    public class DataEasyObby : LDataBlock<DataEasyObby>
    {
        [SerializeField] private LValue<int> _checkpointIndex;

        public static LValue<int> checkpointIndex { get { return instance._checkpointIndex; } }

        protected override void Init()
        {
            base.Init();

            _checkpointIndex = _checkpointIndex ?? new LValue<int>(-1);
        }
    }
}