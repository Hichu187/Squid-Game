using LFramework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResultWin : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Button _btnContinue;
        [SerializeField] private Button _btnHome;
        [SerializeField] private Image _imgCharacter;

        private AdsPlacement _adsPlacementContinue;
        private AdsPlacement _adsPlacementHome;

        private Action _onContinue;

        private void Start()
        {
            _btnContinue.onClick.AddListener(BtnContinue_OnClick);
            _btnHome.onClick.AddListener(BtnHome_OnClick);

            _imgCharacter.sprite = FactoryCharacter.GetSkinCurrent().thumbnail;
        }

        public void Construct(AdsPlacement adsPlacementContinue, AdsPlacement adsPlacementHome, Action onContinue)
        {
            _adsPlacementContinue = adsPlacementContinue;
            _adsPlacementHome = adsPlacementHome;

            _onContinue = onContinue;
        }

        private void BtnContinue_OnClick()
        {
            AdsHelper.ShowInterstitial(_adsPlacementContinue);

            //_onContinue?.Invoke();

            if (DataMainGame.isChallenge)
            {
                SceneLoaderHelper.Load(GameConstants.sceneLobby);
            }
            else
            {
                SceneLoaderHelper.Load(GameConstants.sceneHome);
            }

        }

        private void BtnHome_OnClick()
        {
            AdsHelper.ShowInterstitial(_adsPlacementHome);

            SceneLoaderHelper.Load(GameConstants.sceneHome);
        }
    }
}
