using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickPack
{
    public class FloatingJoystick : Joystick
    {
        Vector3 _originalPos;

        protected override void Start()
        {
            base.Start();

            _originalPos = background.anchoredPosition;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.anchoredPosition = _originalPos;
            base.OnPointerUp(eventData);
        }
    }
}