using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EasyObby_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private UIProgressBar _progressBar;
        [SerializeField] private Button _btnSkipStage;

        public UIProgressBar progressBar { get { return _progressBar; } }

        public event Action eventSkip;

        private void Awake()
        {
            DataEasyObby.checkpointIndex.eventValueChanged += CheckpointIndex_EventValueChanged;
        }

        private void OnDestroy()
        {
            DataEasyObby.checkpointIndex.eventValueChanged -= CheckpointIndex_EventValueChanged;
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
            }, AdsPlacement.EasyObby_Skip_Stage);
        }

        private void UpdateProgress()
        {
            int current = DataEasyObby.checkpointIndex.value;
            int max = FactoryEasyObby.checkpointCount - 1;

            // Update progress bar
            progressBar.UpdateProgress((float)current / max, 2);

            // Update button skip stage active state
            _btnSkipStage.gameObject.SetActive(current < max);
        }
    }
}
