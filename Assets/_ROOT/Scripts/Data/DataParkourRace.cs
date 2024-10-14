using LFramework;
using UnityEngine;

namespace Game
{
    public class DataParkourRace : LDataBlock<DataParkourRace>
    {
        [SerializeField] private int _levelIndex;

        public static int levelIndex { get { return instance._levelIndex; } set { instance._levelIndex = value; } }
    }
}
