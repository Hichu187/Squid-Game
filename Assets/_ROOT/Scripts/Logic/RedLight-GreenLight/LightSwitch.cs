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
        [SerializeField] private TextMeshProUGUI _lightText;
        [SerializeField] private Image _color;

        public TextMeshProUGUI lightText { get { return _lightText; } }
        public Image color { get { return _color; } }

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
            _lightText.text = "Green Light";
            _color.color = Color.green;
        }
        void RedSwitch(Event_RedLightGreenLight_RedLight e)
        {
            _lightText.text = "Red Light";
            _color.color = Color.red;
        }


    }
}
