using Cysharp.Threading.Tasks;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class MarbleShooting_Master : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;
        [SerializeField] private AssetReferenceGameObject _viewResultLose;
        [SerializeField] private Player _player;

        private MarbleShooting_GUI _gui;
        private bool _isFinished = false;
        private bool _isInitialized = false;
        public MarbleShooting_GUI gui { get { return _gui; } }
        public Player player { get { return _player; } }

        private void Start()
        {
            ConstructStart().Forget();
            player.gui.gameObject.SetActive(false);
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<MarbleShooting_GUI>();

            StaticBus<Event_MarbleShooting_Constructed>.Post(null);
        }

        public async UniTaskVoid SpawnResultView()
        {
            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultWin);
        }

        public async UniTaskVoid SpawnResultLose()
        {
            if (_isFinished)
                return;

            _isFinished = true;

            Player.Instance.character.SetEnabled(false);

            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

        }
    }
}
