using Cysharp.Threading.Tasks;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TowerOfHell_Master : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _playerStartPoint;

        [Title("Config")]
        [SerializeField] private float _dropThreshold = 10f;
        [SerializeField] private float _reviveAfterKillDuration = 2f;

        [Space]

        [SerializeField] private AssetReference _reviveView;
        [SerializeField] private AssetReference _guiView;

        private bool _isReviveShowing;

        private TowerOfHell_GUI _gui;

        private Tween _tween;

        private void Awake()
        {
            StaticBus<Event_Player_Die>.Subscribe(StaticBus_Player_Die);
        }

        private void OnDestroy()
        {
            _tween?.Kill();

            StaticBus<Event_Player_Die>.Unsubscribe(StaticBus_Player_Die);
        }

        private void Start()
        {
            Character character = Player.Instance.character;

            RevivePlayer(DataTowerOfHell.playerPosition, DataTowerOfHell.playerRotation);

            character.DisableDieByFalling();
            TowerOfHell_Character_TransformCheck transformCheck = character.gameObjectCached.AddComponent<TowerOfHell_Character_TransformCheck>();

            transformCheck.eventTransformStable += TransformCheck_EventTransformStable;

            SpawnGUIView().Forget();
        }

        private void StaticBus_Player_Die(Event_Player_Die e)
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(_reviveAfterKillDuration, () => { RevivePlayer(Vector3.zero, Quaternion.identity); }, false);
        }

        private void TransformCheck_EventTransformStable(TowerOfHell_Character_TransformCheck transformCheck)
        {
            float dropY = DataTowerOfHell.playerPosition.y - transformCheck.lastStablePosition.y;

            if (dropY > _dropThreshold && DataTowerOfHell.checkpointIndex.value >= 0)
                SpawnReviveView().Forget();

            DataTowerOfHell.playerPosition = transformCheck.lastStablePosition;
            DataTowerOfHell.playerRotation = transformCheck.lastStableRotation;
        }

        private async UniTaskVoid SpawnReviveView()
        {
            if (_isReviveShowing)
                return;

            _isReviveShowing = true;

            View view = await ViewHelper.PushAsync(_reviveView);

            TowerOfHell_Revive revive = view.GetComponent<TowerOfHell_Revive>();
            revive.eventRevive += Revive_EventRevive;
        }

        private async UniTaskVoid SpawnGUIView()
        {
            View view = await ViewHelper.PushAsync(_guiView);

            _gui = view.GetComponent<TowerOfHell_GUI>();

            _gui.eventSkip += GUI_EventSkip;
        }

        private void GUI_EventSkip()
        {
            TowerOfHell_Checkpoint checkpoint = TowerOfHell_Floor_Manager.Instance.checkpoints.GetClamp(DataTowerOfHell.checkpointIndex.value + 1);

            RevivePlayer(checkpoint.transform.position + Vector3.up, checkpoint.transform.rotation);
        }

        private void Revive_EventRevive(bool isSuccess)
        {
            _isReviveShowing = false;

            if (isSuccess)
            {
                TowerOfHell_Checkpoint checkpoint = TowerOfHell_Floor_Manager.Instance.checkpoints.GetClamp(DataTowerOfHell.checkpointIndex.value);

                RevivePlayer(checkpoint.transform.position + Vector3.up, checkpoint.transform.rotation);
            }
            else
            {
                DataTowerOfHell.checkpointPenalty = true;
            }
        }

        private void RevivePlayer(Vector3 position, Quaternion rotation)
        {
            Character character = Player.Instance.character;

            if (position == Vector3.zero)
                character.Revive(_playerStartPoint.position, _playerStartPoint.rotation);
            else
                character.Revive(position, rotation);

        }
    }
}
