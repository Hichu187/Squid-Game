using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class Player : MonoSingleton<Player>
    {
        [Title("Reference")]
        [SerializeField] private Character _character;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private PlayerGUI _gui;
        [SerializeField] private PlayerControl _control;

        protected override bool _dontDestroyOnLoad { get { return false; } }

        public Character character { get { return _character; } }
        public CameraManager cameraManager { get { return _cameraManager; } }
        public PlayerGUI gui { get { return _gui; } }
        public PlayerControl control { get { return _control; } }

        protected override void Awake()
        {
            base.Awake();

            StaticBus<Event_Booster>.Subscribe(StaticBus_Booster);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StaticBus<Event_Booster>.Unsubscribe(StaticBus_Booster);
        }

        private void Start()
        {
            _character.eventRevive += Character_EventRevive;
            _character.eventDie += Character_EventDie;

            // Load player skin
            //_character.rendererComp.LoadSkin(FactoryCharacter.GetSkinCurrent()).Forget();

            /*            if (_character.GetComponent<BladeBall_Player>())
                        {
                            BladeBall_Player p = _character.GetComponent<BladeBall_Player>();
                            p.Init();
                        }*/
        }

        private void Update()
        {
            _gui.SetJetPackFuel(_character.controller.jetPackFuel);
        }

        private void StaticBus_Booster(Event_Booster e)
        {
            if (e.isActive)
            {
                _character.ActiveBooster(e.config.type);

                switch (e.config.type)
                {
                    case BoosterType.JetPack:
                        _gui.SetJetpack();
                        break;
                    case BoosterType.Shoes:
                        _gui.SetJump();
                        break;
                }
            }
            else
            {
                _gui.SetJump();
                _character.DeactiveBooster();
            }
        }

        private void Character_EventDie()
        {
            StaticBus<Event_Player_Die>.Post(new Event_Player_Die(_character));

            //Taptic.Taptic.Medium();
        }

        private void Character_EventRevive()
        {
            StaticBus<Event_Player_Revive>.Post(new Event_Player_Revive(_character));
        }

        public void SetEnabled(bool enabled)
        {
            _character.gameObjectCached.SetActive(enabled);
            _gui.gameObjectCached.SetActive(enabled);
            _control.SetEnabled(enabled);
        }
    }
}