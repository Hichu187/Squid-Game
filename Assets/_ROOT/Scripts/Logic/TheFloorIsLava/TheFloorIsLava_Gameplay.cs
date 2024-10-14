using Cysharp.Threading.Tasks;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_Gameplay : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private TheFloorIsLava_Raiser _raiser;

        [Space]

        [SerializeField] private GameObject _tutorialIndicator;
        [SerializeField] private GameObject _vfxWin;

        private TheFloorIsLava_Master _master;

        private float _lastLavaTime = float.MaxValue;

        private Vector3 _lastPlayerStablePosition = Vector3.zero;
        private Quaternion _lastPlayerStableRotation = Quaternion.identity;

        private void Awake()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Subscribe(StaticBus_TheFloorIsLava_LevelConstructed);
            StaticBus<Event_TheFloorIsLava_Revive>.Subscribe(StaticBus_TheFloorIsLava_Revive);

            StaticBus<Event_Player_Die>.Subscribe(StaticBus_Player_Die);
        }

        private void OnDestroy()
        {
            StaticBus<Event_TheFloorIsLava_LevelConstructed>.Unsubscribe(StaticBus_TheFloorIsLava_LevelConstructed);
            StaticBus<Event_TheFloorIsLava_Revive>.Unsubscribe(StaticBus_TheFloorIsLava_Revive);

            StaticBus<Event_Player_Die>.Unsubscribe(StaticBus_Player_Die);
        }

        private void Start()
        {
            _master = GetComponent<TheFloorIsLava_Master>();

            _raiser.eventTimeRemain += Raiser_EventTimeRemain;
            _raiser.eventComplete += () => { Raiser_EventComplete(); };
        }

        private void StaticBus_TheFloorIsLava_Revive(Event_TheFloorIsLava_Revive e)
        {
            _lastLavaTime = float.MaxValue;

            _raiser.Raise(-10f);

            Player.Instance.character.Revive(_lastPlayerStablePosition, _lastPlayerStableRotation);
        }

        private void StaticBus_TheFloorIsLava_LevelConstructed(Event_TheFloorIsLava_LevelConstructed e)
        {
            LogHelper.TheFloorIsLava_Start(TheFloorIsLava_Static.levelConfig);

            StartGameplay().Forget();
        }

        private void StaticBus_Player_Die(Event_Player_Die e)
        {
            StaticBus<Event_TheFloorIsLava_Result>.Post(new Event_TheFloorIsLava_Result(false));
        }

        private void Raiser_EventComplete()
        {
            Player.Instance.character.Win();

            _vfxWin.Create(Player.Instance.character.transformCached.position, Player.Instance.character.transformCached.rotation);

            StaticBus<Event_TheFloorIsLava_Result>.Post(new Event_TheFloorIsLava_Result(true));
        }

        private void Raiser_EventTimeRemain(float timeRemain)
        {
            // Save player last stable position
            if (Player.Instance.character.motor.GroundingStatus.IsStableOnGround)
            {
                _lastPlayerStablePosition = Player.Instance.character.transformCached.position;
                _lastPlayerStableRotation = Player.Instance.character.transformCached.rotation;
            }

            timeRemain = Mathf.Round(timeRemain * 10f) * 0.1f;

            // Prevent push announcement message too many
            if (timeRemain < _lastLavaTime)
            {
                _master.gui.announcement.PushMesseage($"The floor is lava for {timeRemain.ToString("0.0")} more second{(timeRemain > 1 ? "s" : "")}").Forget();

                _lastLavaTime = timeRemain;
            }
        }

        private void ReviveView_EventRevive(bool isRevive)
        {
            if (!isRevive)
            {
                SceneLoaderHelper.Reload();
            }
            else
            {
                _lastLavaTime = float.MaxValue;

                float y = _lastPlayerStablePosition.y - 10.0f;

                _raiser.Raise(-10f);

                Player.Instance.character.Revive(_lastPlayerStablePosition, _lastPlayerStableRotation);
            }
        }

        private async UniTaskVoid StartGameplay()
        {
            TheFloorIsLava_Level level = TheFloorIsLava_Static.level;

            _lastPlayerStablePosition = level.points.spawnPoint.position;
            _lastPlayerStableRotation = level.points.spawnPoint.rotation;

            Player.Instance.character.Revive(_lastPlayerStablePosition, _lastPlayerStableRotation);
            Player.Instance.character.gameObjectCached.SetActive(true);

            await UniTask.WaitForSeconds(1f, cancellationToken: this.GetCancellationTokenOnDestroy());

            Transform indicator = _tutorialIndicator.Create().transform;
            indicator.localPosition = level.points.highestPoint.position;
            indicator.forward = Vector3.ProjectOnPlane(level.points.highestPoint.position - level.points.cameraPoint.position, Vector3.up);
            indicator.localScale = Vector3.one * 0.5f;

            Sequence sequence = Player.Instance.cameraManager.cameraTutorial.Play(level.points.cameraPoint);

            sequence.onComplete += (() =>
            {
                Player.Instance.SetEnabled(true);
                Player.Instance.character.Revive(_lastPlayerStablePosition, _lastPlayerStableRotation);

                Destroy(indicator.gameObject);

                CountdownLavaDelay(level.lavaDelay).Forget();

                StaticBus<Event_TheFloorIsLava_LevelStart>.Post(null);
            });
        }

        private async UniTask CountdownLavaDelay(int count)
        {
            for (int i = count; i >= 0; i--)
            {
                _master.gui.announcement.PushMessageScale($"The floor will become lava in {i} second{(i > 1 ? "s" : "")}").Forget();

                await UniTask.WaitForSeconds(1, cancellationToken: this.GetCancellationTokenOnDestroy());
            }

            _raiser.Raise(0f);
        }
    }
}