using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class EasyObby_Player : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private GameObject _tutorialIndicator;

        [Title("Config")]
        [SerializeField] private GameObject _playerPrefab;

        private EasyObby_Character _character;

        private EasyObby_Master _master;

        private void Awake()
        {
            _master = GetComponent<EasyObby_Master>();

            StaticBus<Event_EasyObby_Constructed>.Subscribe(StaticBus_EasyObby_Constructed);
        }

        private void OnDestroy()
        {
            StaticBus<Event_EasyObby_Constructed>.Unsubscribe(StaticBus_EasyObby_Constructed);
        }

        private void StaticBus_EasyObby_Constructed(Event_EasyObby_Constructed e)
        {
            InitCharacter();

            UpdateTutorialIndicator();
        }

        private void InitCharacter()
        {
            Player player = _playerPrefab.Create().GetComponent<Player>();

            _character = player.character.gameObjectCached.AddComponent<EasyObby_Character>();
            _character.eventCheckpointChanged += Character_EventCheckpointChanged;

            // Revive character at lastest checkpoint
            _character.checkpointCurrent = EasyObby_StageManager.checkpoints.GetClamp(DataEasyObby.checkpointIndex.value);
            _character.Revive();

            _character.character.eventDie += Character_EventDie;

            _master.gui.eventSkip += GUI_EventSkip;
        }

        private void GUI_EventSkip()
        {
            _character.checkpointCurrent = EasyObby_StageManager.checkpoints.GetClamp(DataEasyObby.checkpointIndex.value + 1);
            _character.Revive();
        }

        private void Character_EventDie()
        {
            // Show ads break
            AdsHelper.ShowInterstitialBreak(AdsPlacement.EasyObby_Die);
        }

        private void Character_EventCheckpointChanged()
        {
            EasyObby_Checkpoint checkpointCurrent = _character.checkpointCurrent;

            if (DataEasyObby.checkpointIndex.value >= checkpointCurrent.index)
                return;

            // Save checkpoint index
            DataEasyObby.checkpointIndex.value = checkpointCurrent.index;

            // Play FX
            if (checkpointCurrent.index > 0)
                checkpointCurrent.PlayFX();

            UpdateTutorialIndicator();

            // Log
            LogHelper.EasyObby_Checkpoint(checkpointCurrent.index);

            // Show ads break
            if (checkpointCurrent.index > 0)
                AdsHelper.ShowInterstitialBreak(AdsPlacement.EasyObby_Checkpoint);

            if (checkpointCurrent.index >= FactoryEasyObby.checkpointCount - 1)
                _master.SpawnResultView().Forget();
        }

        private void UpdateTutorialIndicator()
        {
            EasyObby_Checkpoint checkpointCurrent = _character.checkpointCurrent;
            EasyObby_Checkpoint checkpointNext = _character.checkpointNext;

            // Setup tutorial indicator
            _tutorialIndicator.SetActive(checkpointCurrent.index < EasyObby_StageManager.checkpoints.Length - 1);

            _tutorialIndicator.transform.position = checkpointNext.transformCached.position;
            _tutorialIndicator.transform.forward = (checkpointNext.transformCached.position - checkpointCurrent.transformCached.position).normalized;
        }
    }
}
