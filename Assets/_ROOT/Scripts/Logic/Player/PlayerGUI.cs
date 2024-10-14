using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerGUI : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objJump;
        [SerializeField] private GameObject _objJetPack;
        [SerializeField] private GameObject _objJetPackFuel;

        [Space]

        [SerializeField] private GameObject _objMove;
        [SerializeField] private GameObject _objLook;
        [SerializeField] private GameObject _objAction;
        [SerializeField] private GameObject _objBlock;
        [SerializeField] private GameObject _objSkill;

        JoystickPack.Joystick _joystickMove;

        public GameObject objMove { get { return _objMove; } }
        public GameObject objLook { get { return _objLook; } }
        public GameObject objAction { get { return _objAction; } }
        public GameObject objBlock { get { return _objBlock; } }
        public GameObject objSkill{ get { return _objSkill; } }

        public JoystickPack.Joystick joystickMove { get { if (_joystickMove == null) _joystickMove = _objMove.GetComponent<JoystickPack.Joystick>(); return _joystickMove; } }

        Image _imgJetPackFuelProgress;

        private void Start()
        {
            SetJump();
        }

        public void SetJump()
        {
            _objJump.SetActive(true);

            _objJetPack.SetActive(false);
            _objJetPackFuel.SetActive(false);
        }

        public void SetJetpack()
        {
            _objJetPack.SetActive(true);
            _objJetPackFuel.SetActive(true);

            _objJump.SetActive(false);
        }

        public void SetJetPackFuel(float progress)
        {
            if (_imgJetPackFuelProgress == null)
                _imgJetPackFuelProgress = _objJetPackFuel.transform.GetChild(0).GetComponent<Image>();

            _imgJetPackFuelProgress.fillAmount = progress;
        }
    }
}
