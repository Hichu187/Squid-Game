using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickPack
{
    public class InvisibleJoystick : Joystick
    {
        protected override void Start()
        {
            base.Start();

            background.gameObject.SetActive(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.transform.position = eventData.position;

            base.OnPointerDown(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            Vector2 difference = normalised * magnitude * radius;
            background.anchoredPosition += difference;

            base.HandleInput(magnitude, normalised, radius, cam);
        }
    }
}
