using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResultRevive : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Button _btnRevive;
        [SerializeField] private Button _btnCancel;
        [SerializeField] private Image _imgCharacter;

        private View _view;

        private AdsPlacement _adsPlacementRevive;
        private AdsPlacement _adsPlacementCancel;

        private Action<bool> _onRevive;

        private void Start()
        {
            _view = GetComponent<View>();

            _btnRevive.onClick.AddListener(BtnRevive_OnClick);
            _btnCancel.onClick.AddListener(BtnCancel_OnClick);

            _imgCharacter.sprite = FactoryCharacter.GetSkinCurrent().thumbnail;
        }

        private void BtnRevive_OnClick()
        {
            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                _onRevive?.Invoke(true);

                _view.Close();
            }, _adsPlacementRevive);
        }

        private void BtnCancel_OnClick()
        {
            _onRevive?.Invoke(false);

            AdsHelper.ShowInterstitial(_adsPlacementCancel);

            _view.Close();
        }

        public void Construct(AdsPlacement adsPlacementRevive, AdsPlacement adsPlacementCancel, Action<bool> onRevive)
        {
            _adsPlacementRevive = adsPlacementRevive;
            _adsPlacementCancel = adsPlacementCancel;

            _onRevive = onRevive;
        }
    }
}
