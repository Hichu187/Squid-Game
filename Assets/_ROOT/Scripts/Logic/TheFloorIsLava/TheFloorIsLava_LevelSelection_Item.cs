using LFramework;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TheFloorIsLava_LevelSelection_Item : UIButtonBase
    {
        [Title("Reference")]
        [SerializeField] private Image _imgThumbnail;
        [SerializeField] private GameObject _objAds;
        [SerializeField] private GameObject _objCompleted;
        [SerializeField] private TextMeshProUGUI _txtTitle;

        private TheFloorIsLava_LevelConfig _config;

        private Action<TheFloorIsLava_LevelConfig> _onStart;

        public TheFloorIsLava_LevelConfig config { get { return _config; } }

        private void Start()
        {
            _objAds.SetActive(false);
            _objCompleted.SetActive(false);

            Button.interactable = false;
        }

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            if (_objAds.activeSelf)
            {
                AdsHelper.ShowRewarded((isSuccess) =>
                {
                    if (!isSuccess)
                        return;

                    StartLevel();
                }, AdsPlacement.TheFloorIsLava_Start_Button);
            }
            else
            {
                AdsHelper.ShowInterstitial(AdsPlacement.TheFloorIsLava_Start_Button);

                StartLevel();
            }
        }

        private void StartLevel()
        {
            _onStart?.Invoke(_config);
        }

        public void Construct(TheFloorIsLava_LevelConfig config)
        {
            _config = config;

            _imgThumbnail.sprite = config.thumbnail;

            _txtTitle.text = config.title;
        }

        public void ConstructStart(Action<TheFloorIsLava_LevelConfig> onStart, bool isAds)
        {
            _onStart = onStart;
            _objAds.SetActive(isAds);

            _objCompleted.SetActive(_config.data.isCompleted);

            GetComponentInChildren<AnimationSequence>().enabled = true;

            Button.interactable = true;
        }
    }
}
