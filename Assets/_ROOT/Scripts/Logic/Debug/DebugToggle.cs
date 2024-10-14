using LFramework;
using UnityEngine;

namespace Game
{
    public class DebugToggle : UIButtonBase
    {
        protected virtual bool isOn { get; set; }

        protected override void Awake()
        {
            base.Awake();

            UpdateUI();
        }

        public override void Button_OnClick()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

            base.Button_OnClick();

            isOn = !isOn;

            UpdateUI();

#endif
        }

        void UpdateUI()
        {
            Button.image.color = isOn ? Color.green : Color.red;
        }
    }
}
