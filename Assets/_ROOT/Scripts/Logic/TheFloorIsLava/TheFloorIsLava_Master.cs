using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TheFloorIsLava_Master : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private TheFloorIsLava_LevelConfig _levelConfigTest;

        [Space]

        [SerializeField] private AssetReference _viewLevelSelection;
        [SerializeField] private AssetReference _viewGUI;

        private TheFloorIsLava_LevelConstructor _levelConstructor;
        private TheFloorIsLava_GUI _gui;

        public TheFloorIsLava_GUI gui { get { return _gui; } }

        private void Awake()
        {
            _levelConstructor = GetComponent<TheFloorIsLava_LevelConstructor>();
        }

        private void Start()
        {
#if !UNITY_EDITOR
            _levelConfigTest = null;
#endif

            StartGame().Forget();
        }

        private async UniTaskVoid StartGame()
        {
            Player.Instance.SetEnabled(false);

            View viewGUI = await ViewHelper.PushAsync(_viewGUI);

            _gui = viewGUI.GetComponent<TheFloorIsLava_GUI>();

            if (_levelConfigTest != null)
            {
                StartLevel(_levelConfigTest);
            }
            else
            {
                if (TheFloorIsLava_Static.levelConfig != null)
                    StartLevel(TheFloorIsLava_Static.levelConfig);
                else
                    StartLevelSelection().Forget();
            }
        }

        private TheFloorIsLava_LevelConfig[] GetThreeConfigs()
        {
            List<TheFloorIsLava_LevelConfig> configs = new List<TheFloorIsLava_LevelConfig>(FactoryTheFloorIsLava.levelConfigs);
            TheFloorIsLava_LevelConfig[] results = new TheFloorIsLava_LevelConfig[3];

            results[1] = configs.GetLoop(DataTheFloorIsLava.winCount);
            configs.Remove(results[1]);

            configs.Shuffle();

            results[0] = configs[0];
            configs.RemoveAt(0);

            results[2] = configs[0];

            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].isReward)
                {
                    results[2] = configs[i];
                    return results;
                }
            }

            return results;
        }

        public async UniTaskVoid StartLevelSelection()
        {
            await UniTask.WaitForSeconds(0.5f, cancellationToken: this.GetCancellationTokenOnDestroy());

            View view = await ViewHelper.PushAsync(_viewLevelSelection);

            TheFloorIsLava_LevelSelection levelSelection = view.GetComponent<TheFloorIsLava_LevelSelection>();
            levelSelection.Construct(FactoryTheFloorIsLava.levelConfigs, GetThreeConfigs());

            levelSelection.eventStart += StartLevel;
        }

        public void StartLevel(TheFloorIsLava_LevelConfig levelConfig)
        {
            _levelConstructor.SpawnLevel(levelConfig).Forget();
        }
    }
}
