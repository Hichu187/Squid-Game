using Cysharp.Threading.Tasks;
using LFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class ParkourRace_LevelConstructor : MonoBehaviour
    {
        public async UniTask<ParkourRace_Level> LoadLevelAsync(int index)
        {
            AssetReference levelAsset = FactoryParkourRace.levels.GetLoop(index);

            var handle = levelAsset.InstantiateAsync(transform);

            await handle;

            ParkourRace_Level level = handle.Result.GetComponent<ParkourRace_Level>();

            ParkourRace_Static.level = level;

            GetComponent<AIWaypointManager>().Construct();

            return level;
        }
    }
}
