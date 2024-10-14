using LFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DataTheFloorIsLava : LDataBlock<DataTheFloorIsLava>
    {
        [SerializeField] private Dictionary<string, TheFloorIsLava_LevelData> _levelData;

        [SerializeField] private int _winCount;

        public static int winCount { get { return instance._winCount; } set { instance._winCount = value; } }

        protected override void Init()
        {
            base.Init();

            _levelData = _levelData ?? new Dictionary<string, TheFloorIsLava_LevelData>();
        }

        public static TheFloorIsLava_LevelData GetLevelData(string levelName)
        {
            if (!instance._levelData.ContainsKey(levelName))
                instance._levelData.Add(levelName, new TheFloorIsLava_LevelData());

            return instance._levelData[levelName];
        }
    }
}
