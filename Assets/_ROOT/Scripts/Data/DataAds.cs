using LFramework;
using UnityEngine;

namespace Game
{
    public class DataAds : LDataBlock<DataAds>
    {
        [SerializeField] LValue<bool> _rewardSkip;
        [SerializeField] LValue<bool> _interSkip;
        [SerializeField] LValue<bool> _bannerSkip;

        public static LValue<bool> rewardSkip { get { return instance._rewardSkip; } }
        public static LValue<bool> interSkip { get { return instance._interSkip; } }
        public static LValue<bool> bannerSkip { get { return instance._bannerSkip; } }

        protected override void Init()
        {
            base.Init();

            _rewardSkip = _rewardSkip ?? new LValue<bool>(false);
            _interSkip = _interSkip ?? new LValue<bool>(false);
            _bannerSkip = _bannerSkip ?? new LValue<bool>(false);
        }
    }
}
