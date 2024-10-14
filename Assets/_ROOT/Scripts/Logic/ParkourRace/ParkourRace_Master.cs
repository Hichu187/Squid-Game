using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class ParkourRace_Master : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private AssetReference _viewGUI;
        [SerializeField] private AssetReference _viewResultWin;
        [SerializeField] private AssetReference _viewResultLose;

        private ParkourRace_GUI _gui;
        private ParkourRace_LevelConstructor _levelConstrcutor;

        private int _characterFinishCount = 0;
        private bool _isFinished = false;
        private bool _isInitialized = false;

        public ParkourRace_GUI gui { get { return _gui; } }

        private void Awake()
        {
            StaticBus<Event_ParkourRace_Checkpoint>.Subscribe(StaticBus_ParkourRace_Checkpoint);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ParkourRace_Checkpoint>.Unsubscribe(StaticBus_ParkourRace_Checkpoint);
        }

        private async void Start()
        {
            StaticBus<Event_ParkourRace_Construct_Start>.Post(null);

            _levelConstrcutor = GetComponentInChildren<ParkourRace_LevelConstructor>();

            await Construct().AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow();
        }

        private async UniTask Construct()
        {
            // Spawn GUI
            UniTask<View> viewTask = ViewHelper.PushAsync(_viewGUI);

            // Spawn level
            UniTask<ParkourRace_Level> levelTask = _levelConstrcutor.LoadLevelAsync(DataParkourRace.levelIndex);

            // Wait for all task to complete
            var (view, level) = await UniTask.WhenAll(viewTask, levelTask).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());

            _gui = view.GetComponent<ParkourRace_GUI>();

            StaticBus<Event_ParkourRace_Construct_Complete>.Post(null);

            await UniTask.WaitForSeconds(1f);

            await _gui.announcementCountdown.Countdown();

            StaticBus<Event_ParkourRace_Gameplay_Start>.Post(null);

            // Log
            LogHelper.ParkourRace_Start(DataParkourRace.levelIndex);

            _isInitialized = true;
        }

        private void StaticBus_ParkourRace_Checkpoint(Event_ParkourRace_Checkpoint e)
        {
            if (!_isInitialized)
                return;

            // Check if a character reach last checkpoint
            if (e.checkpoint.index != ParkourRace_Static.level.checkpoints.Last().index)
                return;

            if (e.character.isPlayer)
            {
                if (_characterFinishCount == 0)
                    Win().AttachExternalCancellation(destroyCancellationToken).Forget();
                else
                    Lose().AttachExternalCancellation(destroyCancellationToken).Forget();
            }
            else
            {
                _characterFinishCount++;
            }
        }

        private async UniTask Win()
        {
            if (_isFinished)
                return;

            StaticBus<Event_ParkourRace_Gameplay_End>.Post(null);

            _isFinished = true;

            // Log win
            LogHelper.ParkourRace_Win(DataParkourRace.levelIndex);

            DataParkourRace.levelIndex++;

            Player.Instance.character.SetEnabled(false);
            Player.Instance.character.animator.PlayWin();

            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultWin);

            view.GetComponent<ResultWin>().Construct(AdsPlacement.ParkourRace_Win_Continue, AdsPlacement.ParkourRace_Win_Home, SceneLoaderHelper.Reload);
        }

        private async UniTask Lose()
        {
            if (_isFinished)
                return;

            StaticBus<Event_ParkourRace_Gameplay_End>.Post(null);

            _isFinished = true;

            Player.Instance.character.SetEnabled(false);

            await UniTask.WaitForSeconds(1f);

            View view = await ViewHelper.PushAsync(_viewResultLose);

            view.GetComponent<ResultLose>().Construct(AdsPlacement.ParkourRace_Lose_Retry, AdsPlacement.ParkourRace_Lose_Home);
        }
    }
}
