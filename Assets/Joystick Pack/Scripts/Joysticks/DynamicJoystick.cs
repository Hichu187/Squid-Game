using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickPack
{
    public class DynamicJoystick : Joystick
    {
        [SerializeField] private float _moveThreshold = 1;

        Vector3 _originPos;

        protected override void Start()
        {
            base.Start();

            _originPos = background.anchoredPosition;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);

            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.anchoredPosition = _originPos;

            base.OnPointerUp(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > _moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - _moveThreshold) * radius;
                background.anchoredPosition += difference;
            }

            base.HandleInput(magnitude, normalised, radius, cam);
        }
    }
}