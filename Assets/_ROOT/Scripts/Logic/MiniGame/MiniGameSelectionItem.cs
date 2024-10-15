using LFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MiniGameSelectionItem : UIButtonBase
    {
        [Title("Reference")]
        [SerializeField] private TextMeshProUGUI _txtTitle;
        [SerializeField] private Image _imgThumbnail;
        [SerializeField] private TextMeshProUGUI _txtOnLight;
        [SerializeField] private GameObject _objMostPlayed;
        [SerializeField] private GameObject _objComingSoon;
        [SerializeField] private GameObject _objStatus;

        private MiniGameConfig _config;

        private void ConstructProgress()
        {
            MiniGameSelectionItemProgress progress = GetComponent<MiniGameSelectionItemProgress>();

            switch (_config.type)
            {
                case MiniGameType.EasyObby:
                    progress.Construct((float)DataEasyObby.checkpointIndex.value / (FactoryEasyObby.checkpointCount - 1), 2);
                    break;
                case MiniGameType.TowerOfHell:
                    progress.Construct(TowerOfHell_Helper.GetProgress());
                    break;
                case MiniGameType.TheFloorIsLava:
                    progress.Construct(TheFloorIsLava_Helper.GetProgress());
                    break;
                case MiniGameType.ParkourRace:
                    progress.Construct((float)DataParkourRace.levelIndex / FactoryParkourRace.levels.Length);
                    break;
                default:
                    progress.Hide();
                    break;
            }
        }

        public void Construct(MiniGameConfig config)
        {
            _config = config;

            _txtTitle.text = config.title;
            _imgThumbnail.sprite = config.thumbnail;

            _objMostPlayed.SetActive(config.isMostPlayed);

            _txtOnLight.text = $"{UtilsNumber.Format(config.isMostPlayed ? Random.Range(1000, 5000) : Random.Range(100, 900))} OnLight";

            _objComingSoon.SetActive(config.isComingSoon);

            _objStatus.SetActive(!config.isComingSoon);

            ConstructProgress();

#if UNITY_IOS
            if (config.isComingSoon)
                gameObjectCached.SetActive(false);
#endif
        }

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            if (_objComingSoon.activeInHierarchy)
            {
                UINotificationText.Push("This game is coming soon!");
            }
            else
            {
                MiniGameStatic.current = _config;

                AdsHelper.ShowInterstitial($"Home_{_config.sceneName}");

                LogHelper.EnterMiniGame(_config);

                SceneLoaderHelper.Load(_config.sceneName);
            }
        }
    }
}
