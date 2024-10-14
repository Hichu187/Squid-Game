using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TheFloorIsLava_Result : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private AssetReference _viewResultLose;
        [SerializeField] private AssetReference _viewResultWin;

        private bool _isEnded = false;

        private void Awake()
        {
            StaticBus<Event_TheFloorIsLava_Result>.Subscribe(StaticBus_TheFloorIsLava_Result);
        }

        private void OnDestroy()
        {
            StaticBus<Event_TheFloorIsLava_Result>.Unsubscribe(StaticBus_TheFloorIsLava_Result);
        }

        private void StaticBus_TheFloorIsLava_Result(Event_TheFloorIsLava_Result e)
        {
            if (_isEnded)
                return;

            _isEnded = true;

            if (e.isWin)
            {
                DataTheFloorIsLava.winCount++;

                TheFloorIsLava_Static.levelConfig.data.Complete();

                LogHelper.TheFloorIsLava_Win(TheFloorIsLava_Static.levelConfig);

                TheFloorIsLava_Static.levelConfig = null;

                WinAsync().Forget();
            }
            else
            {
                LoseAsync().Forget();
            }
        }

        private async UniTaskVoid WinAsync()
        {
            await UniTask.WaitForSeconds(2.0f);

            View view = await ViewHelper.PushAsync(_viewResultWin);

            view.GetComponent<ResultWin>().Construct(AdsPlacement.TheFloorIsLava_Win_Continue, AdsPlacement.TheFloorIsLava_Win_Home, () => { SceneLoaderHelper.Reload(); });
        }

        private async UniTaskVoid LoseAsync()
        {
            await UniTask.WaitForSeconds(1.0f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

            view.GetComponent<ResultRevive>().Construct(AdsPlacement.TheFloorIsLava_Revive_OK, AdsPlacement.TheFloorIsLava_Revive_Cancel, ReviveView_EventRevive);
        }

        private void ReviveView_EventRevive(bool isRevive)
        {
            if (!isRevive)
            {
                SceneLoaderHelper.Reload();
            }
            else
            {
                _isEnded = false;

                StaticBus<Event_TheFloorIsLava_Revive>.Post(null);
            }
        }
    }
}
