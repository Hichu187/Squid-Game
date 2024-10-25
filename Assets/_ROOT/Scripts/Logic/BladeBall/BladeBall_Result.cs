using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class BladeBall_Result : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private AssetReference _viewResultLose;
        [SerializeField] private AssetReference _viewResultWin;

        private bool _isEnded = false;

        private void Awake()
        {
            StaticBus<Event_BladeBall_Result>.Subscribe(StaticBus_BladeBall_Result);
        }

        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_Result>.Unsubscribe(StaticBus_BladeBall_Result);
        }

        private void StaticBus_BladeBall_Result(Event_BladeBall_Result e)
        {
            if (_isEnded)
                return;

            _isEnded = true;

            if (e.isWin)
            {
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

            view.GetComponent<ResultWin>().Construct(AdsPlacement.BladeBall_Win_Continue, AdsPlacement.BladeBall_Win_Home, () => { SceneLoaderHelper.Reload(); });
        }

        private async UniTaskVoid LoseAsync()
        {
            await UniTask.WaitForSeconds(1.0f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

            view.GetComponent<ResultRevive>().Construct(AdsPlacement.BladeBall_Revive_OK, AdsPlacement.BladeBall_Revive_Cancel, ReviveView_EventRevive);
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

                StaticBus<Event_BladeBall_Revive>.Post(null);
            }
        }
    }
}
