using Cysharp.Threading.Tasks;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class ColorBlock_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private ColorBlock_PlatformManager _platformManager;

        [Title("Config")]
        [SerializeField] private float _countdownStart;

        [Space]

        [SerializeField] private int _roundCount;
        [SerializeField] private float _roundDuration;
        [SerializeField] private float _roundEndDuration;

        [Space]

        [SerializeField] private AssetReferenceGameObject _viewResultLose;
        [SerializeField] private AssetReferenceGameObject _viewResultWin;
        [SerializeField] private AssetReferenceGameObject _viewGUI;

        private int _roundIndex;

        private Tween _tween;

        private ColorBlock_GUI _gui;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private void Awake()
        {
            StaticBus<Event_Player_Die>.Subscribe(StaticBus_Player_Die);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Player_Die>.Unsubscribe(StaticBus_Player_Die);

            _tween?.Kill();

            _cts?.Cancel();
        }

        private void Start()
        {
            StartGame().Forget();
        }

        private void StaticBus_Player_Die(Event_Player_Die e)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            CreateViewRevive().Forget();
        }

        private async UniTask CreateViewRevive()
        {
            View view = await ViewHelper.PushAsync(_viewResultLose);

            view.GetComponent<ResultRevive>().Construct(AdsPlacement.ColorBlock_Revive_OK, AdsPlacement.ColorBlock_Revive_Cancel, ReviveView_EventRevive);
        }

        private void ReviveView_EventRevive(bool isSuccess)
        {
            if (!isSuccess)
            {
                SceneLoaderHelper.Reload();
            }
            else
            {
                Player.Instance.character.Revive(Vector3.zero, Quaternion.identity);

                PlayRound().Forget();
            }
        }

        private async UniTaskVoid PlayRound()
        {
            StaticBus<Event_ColorBlock_PlatformRestore>.Post(null);

            ColorBlock_ColorConfig colorConfig = FactoryColorBlock.colorConfigs.GetRandom();

            _platformManager.ShufflePlatformColor(colorConfig, Mathf.Max(_roundCount - _roundIndex, 3));

            StaticBus<Event_ColorBlock_RoundStart>.Post(null);

            await _gui.colorNotify.Countdown(colorConfig, _roundDuration).AttachExternalCancellation(_cts.Token);

            StaticBus<Event_ColorBlock_PlatformHide>.Post(new Event_ColorBlock_PlatformHide(colorConfig));

            await UniTask.WaitForSeconds(_roundEndDuration, cancellationToken: _cts.Token);

            _roundIndex++;

            if (_roundIndex >= _roundCount)
            {
                EndGame().Forget();
            }
            else
            {
                PlayRound().Forget();
            }
        }

        private async UniTask StartGame()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);
            _gui = view.GetComponent<ColorBlock_GUI>();

            float time = _countdownStart;

            while (time > 0f)
            {
                _gui.announcement.PushMessageScale($"Game start in {time} second{(time > 1f ? "s" : "")}").Forget();

                time -= 1.0f;

                await UniTask.WaitForSeconds(1, cancellationToken: _cts.Token);
            }

            PlayRound().Forget();
        }

        private async UniTaskVoid EndGame()
        {
            View view = await ViewHelper.PushAsync(_viewResultWin);

            view.GetComponent<ResultWin>().Construct(AdsPlacement.ColorBlock_Win_Continue, AdsPlacement.ColorBlock_Win_Home, () => { SceneLoaderHelper.Reload(); });
        }
    }
}
