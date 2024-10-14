using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TowerOfHell_Revive : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Button _btnRevive;
        [SerializeField] private Button _btnCancel;

        private View _view;
        private TowerOfHell_Revive_Countdown _countdown;

        public event Action<bool> eventRevive;

        bool _isRevive = false;

        private void Start()
        {
            _view = GetComponent<View>();
            _view.onCloseEnd.AddListener(View_OnCloseEnd);

            _countdown = GetComponent<TowerOfHell_Revive_Countdown>();

            _btnRevive.onClick.AddListener(BtnRevive_OnClick);
            _btnCancel.onClick.AddListener(BtnCancel_OnClick);

            TimeManager.Pause();
        }

        private void OnDestroy()
        {
            TimeManager.Resume();
        }

        private void View_OnCloseEnd()
        {
            eventRevive?.Invoke(_isRevive);
        }

        private void BtnRevive_OnClick()
        {
            _countdown.Stop();

            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                _isRevive = true;

                _view.Close();
            }, AdsPlacement.TowerOfHell_Revive_OK);
        }

        private void BtnCancel_OnClick()
        {
            AdsHelper.ShowInterstitial(AdsPlacement.TowerOfHell_Revive_Cancel);

            _view.Close();
        }
    }
}
