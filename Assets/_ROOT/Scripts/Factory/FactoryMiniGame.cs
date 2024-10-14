using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryMiniGame : ScriptableObjectSingleton<FactoryMiniGame>
    {
        [AssetList(Path = "_ROOT/Configs/MiniGame", AutoPopulate = false)]
        [SerializeField] private MiniGameConfig[] _configs;

        [SerializeField] private MiniGameConfig _easyObby;
        [SerializeField] private MiniGameConfig _theFloorIsLava;
        [SerializeField] private MiniGameConfig _colorBlock;
        [SerializeField] private MiniGameConfig _bladeBall;

        public static MiniGameConfig[] configs => instance._configs;
        public static MiniGameConfig easyObby => instance._easyObby;
        public static MiniGameConfig theFloorIsLava => instance._theFloorIsLava;
        public static MiniGameConfig colorBlock => instance._colorBlock;
        public static MiniGameConfig bladeBall => instance._bladeBall;
    }
}
