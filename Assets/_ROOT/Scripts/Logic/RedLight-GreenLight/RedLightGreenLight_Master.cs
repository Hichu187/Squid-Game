using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class RedLightGreenLight_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;
        [SerializeField] private AssetReferenceGameObject _viewResultLose;
        [SerializeField] private Player _player;

        private RedLightGreenLight_GUI _gui;

        private int _characterFinishCount = 0;
        private bool _isFinished = false;
        private bool _isInitialized = false;

        public RedLightGreenLight_GUI gui { get { return _gui; } }
        public Player player { get { return _player; } }

        private void Start()
        {
            ConstructStart().Forget();
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<RedLightGreenLight_GUI>();

            StaticBus<Event_RedLightGreenLight_Constructed>.Post(null);
        }

        public async UniTaskVoid SpawnResultView()
        {
            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultWin);

        }

        private async UniTask Lose()
        {
            if (_isFinished)
                return;

            //StaticBus<Event_ParkourRace_Gameplay_End>.Post(null);

            _isFinished = true;

            Player.Instance.character.SetEnabled(false);

            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

        }

    }
}
