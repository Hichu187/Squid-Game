using LFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LightSwitch : MonoBehaviour
    {
        public TextMeshProUGUI lightText;
        public Image color;

        private void Awake()
        {
            StaticBus<Event_RedLightGreenLight_GreenLight>.Subscribe(GreenSwitch);
            StaticBus<Event_RedLightGreenLight_RedLight>.Subscribe(RedSwitch);
        }
        private void Start()
        {
            this.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            StaticBus<Event_RedLightGreenLight_GreenLight>.Subscribe(GreenSwitch);
            StaticBus<Event_RedLightGreenLight_RedLight>.Subscribe(RedSwitch);
        }

        void GreenSwitch(Event_RedLightGreenLight_GreenLight e)
        {
            lightText.text = "Green Light";
            if(color != null) color.color = Color.green;
        }
        void RedSwitch(Event_RedLightGreenLight_RedLight e)
        {
            lightText.text = "Red Light";
            if (color != null) color.color = Color.red;
        }
    }
}
