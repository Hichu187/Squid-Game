using LFramework;
using UnityEngine;

namespace Game
{
    public class FactoryTheFloorIsLava : ScriptableObjectSingleton<FactoryTheFloorIsLava>
    {
        [SerializeField] private TheFloorIsLava_LevelConfig[] _levelConfigs;

        public static TheFloorIsLava_LevelConfig[] levelConfigs => instance._levelConfigs;
    }
}
