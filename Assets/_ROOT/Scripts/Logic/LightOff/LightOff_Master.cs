using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class LightOff_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;
        [SerializeField] private AssetReferenceGameObject _viewResultLose;
        [SerializeField] private Player _player;
        [SerializeField] private Camera _mainCamera;

        private LightOff_GUI _gui;

        private int _characterFinishCount = 0;
        private bool _isFinished = false;
        private bool _isInitialized = false;

        public LightOff_GUI gui { get { return _gui; } }
        public Player player { get { return _player; } }
        public Camera mainCamera { get { return _mainCamera; } }

        private void Start()
        {
            ConstructStart().Forget();
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<LightOff_GUI>();

            StaticBus<Event_LightOff_Constructed>.Post(null);
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
