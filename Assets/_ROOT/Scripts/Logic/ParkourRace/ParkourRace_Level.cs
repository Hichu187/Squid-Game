using UnityEngine;

namespace Game
{
    public class ParkourRace_Level : MonoBehaviour
    {
        private ParkourRace_Checkpoint[] _checkpoints;

        public ParkourRace_Checkpoint[] checkpoints { get { return _checkpoints; } }

        private void Awake()
        {
            _checkpoints = GetComponentsInChildren<ParkourRace_Checkpoint>();

            for (int i = 0; i < _checkpoints.Length; i++)
            {
                _checkpoints[i].SetIndex(i);
            }
        }
    }
}
