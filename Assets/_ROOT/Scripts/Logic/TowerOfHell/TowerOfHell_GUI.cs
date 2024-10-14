using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TowerOfHell_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private UIProgressBar _progressBar;
        [SerializeField] private Button _btnSkipStage;

        public UIProgressBar progressBar { get { return _progressBar; } }

        public event Action eventSkip;

        private void Awake()
        {
            DataTowerOfHell.checkpointIndex.eventValueChanged += CheckpointIndex_EventValueChanged;
        }

        private void OnDestroy()
        {
            DataTowerOfHell.checkpointIndex.eventValueChanged -= CheckpointIndex_EventValueChanged;
        }

        private void Start()
        {
            _btnSkipStage.onClick.AddListener(BtnSkipStage_OnClick);

            UpdateProgress();
        }

        private void CheckpointIndex_EventValueChanged(int index)
        {
            UpdateProgress();
        }

        private void BtnSkipStage_OnClick()
        {
            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                eventSkip?.Invoke();
            }, AdsPlacement.TowerOfHell_Skip_Stage);
        }

        private void UpdateProgress()
        {
            float progress = TowerOfHell_Helper.GetProgress();

            // Update progress bar
            progressBar.UpdateProgress(progress);

            // Update button skip stage active state
            _btnSkipStage.gameObject.SetActive(progress < 1f);
        }
    }
}
