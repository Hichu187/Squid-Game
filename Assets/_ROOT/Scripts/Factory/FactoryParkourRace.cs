using LFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class FactoryParkourRace : ScriptableObjectSingleton<FactoryParkourRace>
    {
        [SerializeField] private AssetReference[] _levels;

        public static AssetReference[] levels => instance._levels;
    }
}
