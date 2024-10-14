using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class AdsBanner : MonoBehaviour
    {
        Tween _tween;

        private void Awake()
        {
            DataAds.bannerSkip.eventValueChanged += BannerSkip_EventValueChanged;
        }

        private void OnDestroy()
        {
            _tween?.Kill();

            DataAds.bannerSkip.eventValueChanged -= BannerSkip_EventValueChanged;
        }

        private void Start()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(10f, CheckShowBanner, false)
                              .SetLoops(-1, LoopType.Restart);
        }

        private void BannerSkip_EventValueChanged(bool isSkip)
        {
            CheckShowBanner();
        }

        private void CheckShowBanner()
        {
            if (DataAds.bannerSkip.value)
                AdsHelper.HideBanner();
            else
                AdsHelper.ShowBanner();
        }
    }
}
