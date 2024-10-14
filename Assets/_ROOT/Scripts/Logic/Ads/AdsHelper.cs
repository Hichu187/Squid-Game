using Cysharp.Threading.Tasks;
using LFramework;
using System;

namespace Game
{
    public static class AdsHelper
    {
        public static void ShowBanner()
        {
            //BounceAdsSdk.ShowBanner();
        }

        public static void HideBanner()
        {
            //BounceAdsSdk.HideBanner();
        }

        public static async void ShowInterstitialBreak(AdsPlacement placement = AdsPlacement.Unset)
        {
            if (!IsInterstitialOKToShow())
                return;

/*            if (!RemoteConfig.CONFIG.adsBreakEnable)
            {
                ShowInterstitial(placement);
                return;
            }*/

            View view = await ViewHelper.PushAsync(FactoryPrefab.adsBreakPopup);

            await UniTask.WaitForSeconds(2f);

            view.Close();

            ShowInterstitial(placement);
        }

        public static void ShowInterstitial(AdsPlacement placement = AdsPlacement.Unset)
        {
            ShowInterstitial(placement.ToString().ToLower());
        }

        public static void ShowInterstitial(string placement)
        {
            MiniGameConfig miniGame = MiniGameStatic.current ?? FactoryMiniGame.easyObby;

            //BounceAdsSdk.ShowInterstitial(placement, "minigame_title", miniGame.title);
        }

        public static void ShowRewarded(Action<bool> onReward, AdsPlacement placement = AdsPlacement.Unset)
        {
            Action<bool> callback = onReward;

#if UNITY_EDITOR || DEVELOPMENT_BUILD

            // Check if reward ads is removed
            if (DataAds.rewardSkip.value)
            {
                callback?.Invoke(true);
                return;
            }

#endif

            // Check if reward ads is available
/*            if (!BounceAdsSdk.IsRewardedReady())
            {
                UINotificationText.Push("No ads available at the moment, try again later!");

                callback?.Invoke(false);
            }
            else
            {
                BounceAdsSdk.ShowRewarded(callback, placement.ToString().ToLower());
            }*/
        }

        public static bool IsInterstitialOKToShow()
        {
            return true;
        }
    }
}
