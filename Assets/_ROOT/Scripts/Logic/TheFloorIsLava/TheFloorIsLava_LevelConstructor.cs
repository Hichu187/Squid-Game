using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TheFloorIsLava_LevelConstructor : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _root;

        private TheFloorIsLava_Level _level;

        private void OnDestroy()
        {
            if (_level != null)
                Addressables.ReleaseInstance(_level.gameObjectCached);
        }

        public async UniTaskVoid SpawnLevel(TheFloorIsLava_LevelConfig levelConfig)
        {
            var handle = Addressables.InstantiateAsync(levelConfig.prefab, _root);

            await handle;

            _level = handle.Result.GetComponent<TheFloorIsLava_Level>();

            TheFloorIsLava_Static.level = _level;
            TheFloorIsLava_Static.levelConfig = levelConfig;

            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Post(null);
        }
    }
}