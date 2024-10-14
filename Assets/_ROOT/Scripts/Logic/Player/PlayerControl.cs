using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class PlayerControl : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Player _player;

        [Title("Config")]
        [SerializeField] private float _lookSensitive = 20f;
        [SerializeField] private bool _useMobileControl = false;

        private CharacterKCInputPlayer _input = new CharacterKCInputPlayer();

        private bool _inputActionDown = false;
        private bool _inputActionConsumed = true;
        private float _inputLookX;
        private float _inputLookY;
        private float _inputMoveX;
        private float _inputMoveY;

        public bool useMobileControl { get { return _useMobileControl; } }

        #region MonoBehaviour

        private void Start()
        {
#if !UNITY_EDITOR
            _useMobileControl = true;
#endif

            _player.character.eventDie += Character_EventDie;

            Init();
        }

        private void Character_EventDie()
        {
            _input.Reset();
        }

        private void Update()
        {
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        #endregion

        private void Init()
        {
            UIPointerDrag look = _player.gui.objLook.GetComponent<UIPointerDrag>();
            look.eventDrag += UILook_EventDrag;
            look.eventDragEnd += UILook_EventDragEnd;

            UIPointerClick action = _player.gui.objAction.GetComponent<UIPointerClick>();
            action.eventDown += UIAction_EventDown;
            action.eventUp += UIAction_EventUp;
        }

        private void UILook_EventDrag(PointerEventData e)
        {
            _inputLookX = e.delta.x / Screen.dpi;
            _inputLookY = e.delta.y / Screen.dpi;
        }

        private void UILook_EventDragEnd(PointerEventData e)
        {
            _inputLookX = 0f;
            _inputLookY = 0f;
        }

        private void UIAction_EventDown()
        {
            _inputActionDown = true;
            _inputActionConsumed = false;
        }

        private void UIAction_EventUp()
        {
            _inputActionDown = false;
        }

        private void HandleCameraInput()
        {
            Vector3 lookInputVector = Vector3.zero;

            // Create the look input vector for the camera
            if (!_useMobileControl)
            {
                lookInputVector.x = Input.GetAxis("HorizontalArrow");
                lookInputVector.y = Input.GetAxis("VerticalArrow");
            }
            else
            {
                lookInputVector.x = _inputLookX;
                lookInputVector.y = _inputLookY;

                _inputLookX = 0f;
                _inputLookY = 0f;
            }

            lookInputVector *= _lookSensitive;

            // Apply inputs to the camera
            _player.cameraManager.UpdateInput(lookInputVector);
        }

        private void HandleCharacterInput()
        {
            if (_useMobileControl)
            {
                _inputMoveX = _player.gui.joystickMove.Horizontal;
                _inputMoveY = _player.gui.joystickMove.Vertical;
            }
            else
            {
                _inputMoveX = Input.GetAxis("Horizontal");
                _inputMoveY = Input.GetAxis("Vertical");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _inputActionDown = true;
                    _inputActionConsumed = false;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    _inputActionDown = true;
                }
                else
                {
                    _inputActionDown = false;
                    _inputActionConsumed = true;
                }
            }

            _input.moveAxisForward = _inputMoveY;
            _input.moveAxisRight = _inputMoveX;

            if (_player.character.boosterJetpackEnabled)
            {
                _input.jumpDown = false;
                _input.jetpackDown = _inputActionDown;
            }
            else
            {
                _input.jetpackDown = false;

                if (!_inputActionConsumed)
                {
                    _input.jumpDown = true;
                    _inputActionConsumed = true;
                }
                else
                {
                    _input.jumpDown = false;
                }
            }

            _input.cameraRotation = _player.cameraManager.cameraTransform.rotation;

            // Apply inputs to character
            _player.character.controller.SetInputs(ref _input);
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}