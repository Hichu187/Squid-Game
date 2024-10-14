namespace Game
{
    public class DebugToggleAdsInterSkip : DebugToggle
    {
        protected override bool isOn { get { return DataAds.interSkip.value; } set { DataAds.interSkip.value = value; } }
    }
}
