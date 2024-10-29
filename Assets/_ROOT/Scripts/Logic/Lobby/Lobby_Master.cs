using Cysharp.Threading.Tasks;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class Lobby_Master : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private Player _player;

        private Lobby_GUI _gui;
        public Lobby_GUI gui { get { return _gui; } }
        public Player player { get { return _player; } }

        private void Start()
        {
            ConstructStart().Forget();
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<Lobby_GUI>();

            StaticBus<Event_Lobby_Constructed>.Post(null);
        }
    }

}
