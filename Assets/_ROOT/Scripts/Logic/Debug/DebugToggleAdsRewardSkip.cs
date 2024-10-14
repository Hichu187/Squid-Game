namespace Game
{
    public class DebugToggleAdsRewardSkip : DebugToggle
    {
        protected override bool isOn { get { return DataAds.rewardSkip.value; } set { DataAds.rewardSkip.value = value; } }
    }
}