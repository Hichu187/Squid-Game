using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BoosterButton : UIButtonBase
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objAds;
        [SerializeField] private Image _imgFill;

        [Title("Config")]
        [SerializeField] private BoosterConfig _config;
        [SerializeField] private AdsPlacement _adsPlacement;

        private Tween _tween;

        protected override void Awake()
        {
            base.Awake();

            StaticBus<Event_Booster>.Subscribe(StaticBus_Booster);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Booster>.Unsubscribe(StaticBus_Booster);

            _tween?.Kill();
        }

        private void Start()
        {
            ResetGUI();
        }

        private void StaticBus_Booster(Event_Booster e)
        {
            if (e.isActive)
                gameObjectCached.SetActive(e.config == _config);
            else
                gameObjectCached.SetActive(true);
        }

        private void ResetGUI()
        {
            _objAds.SetActive(true);
            _imgFill.gameObject.SetActive(false);
        }

        private void StartCountdown()
        {
            _objAds.SetActive(false);
            _imgFill.gameObject.SetActive(true);

            _tween?.Kill();
            _tween = _imgFill.DOFillAmount(0f, _config.duration)
                             .SetUpdate(false)
                             .ChangeStartValue(1f);

            _tween.onComplete += () =>
            {
                ResetGUI();

                StaticBus<Event_Booster>.Post(new Event_Booster(_config, false));
            };
        }

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            if (!_objAds.activeSelf)
                return;

            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                StaticBus<Event_Booster>.Post(new Event_Booster(_config, true));

                StartCountdown();
            }, _adsPlacement);
        }
    }
}
