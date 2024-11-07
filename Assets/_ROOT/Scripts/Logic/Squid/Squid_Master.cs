using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class Squid_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;
        [SerializeField] private AssetReferenceGameObject _viewResultLose;
        [SerializeField] private Player _player;

        private Squid_GUI _gui;

        public Squid_GUI gui { get { return _gui; } }
        public Player player { get { return _player; } }

        private void Start()
        {
            ConstructStart().Forget();
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<Squid_GUI>();

            StaticBus<Event_Squid_Constructed>.Post(null);
        }

        public async UniTaskVoid SpawnResultView()
        {
            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultWin);

        }

        public async UniTaskVoid Lose()
        {

            Player.Instance.character.SetEnabled(false);

            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

        }
    }
}
