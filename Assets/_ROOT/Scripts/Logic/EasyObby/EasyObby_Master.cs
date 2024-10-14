using Sirenix.OdinInspector;
using UnityEngine;
using LFramework;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Game
{
    public class EasyObby_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;

        private EasyObby_GUI _gui;

        public EasyObby_GUI gui { get { return _gui; } }

        private void Start()
        {
            ConstructStart().Forget();
        }

        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<EasyObby_GUI>();

            StaticBus<Event_EasyObby_Constructed>.Post(null);
        }

        public async UniTaskVoid SpawnResultView()
        {
            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultWin);

            view.GetComponent<ResultWin>().Construct(AdsPlacement.EasyObby_Win_Continue, AdsPlacement.EasyObby_Win_Home, () => { SceneLoaderHelper.Load(GameConstants.sceneHome); });
        }
    }
}
