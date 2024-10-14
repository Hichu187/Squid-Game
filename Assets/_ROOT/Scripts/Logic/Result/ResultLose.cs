using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResultLose : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Button _btnRetry;
        [SerializeField] private Button _btnHome;
        [SerializeField] private Image _imgCharacter;

        private View _view;

        private AdsPlacement _adsPlacementRetry;
        private AdsPlacement _adsPlacementHome;

        private void Start()
        {
            _view = GetComponent<View>();

            _btnRetry.onClick.AddListener(BtnRetry_OnClick);
            _btnHome.onClick.AddListener(BtnHome_OnClick);

            _imgCharacter.sprite = FactoryCharacter.GetSkinCurrent().thumbnail;
        }

        private void BtnRetry_OnClick()
        {
            AdsHelper.ShowInterstitial(_adsPlacementRetry);

            SceneLoaderHelper.Reload();
        }

        private void BtnHome_OnClick()
        {
            AdsHelper.ShowInterstitial(_adsPlacementHome);

            SceneLoaderHelper.Load(GameConstants.sceneHome);
        }

        public void Construct(AdsPlacement adsPlacementRetry, AdsPlacement adsPlacementHome)
        {
            _adsPlacementRetry = adsPlacementRetry;
            _adsPlacementHome = adsPlacementHome;
        }
    }
}
