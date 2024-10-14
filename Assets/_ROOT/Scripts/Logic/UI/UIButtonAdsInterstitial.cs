using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class UIButtonAdsInterstitial : UIButtonBase
    {
        [Title("Config")]
        [SerializeField] private AdsPlacement _placement;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            AdsHelper.ShowInterstitial(_placement);
        }
    }
}
