using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShopSkinPreview : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Image _imgIcon;

        [SerializeField] private GameObject _objUnlock;
        [SerializeField] private GameObject _objEquip;
        [SerializeField] private GameObject _objEquipped;

        [Space]

        [SerializeField] private ShopSkin _shopSkin;

        CharacterSkinConfig _config;

        private void Awake()
        {
            _shopSkin.eventSelected += ShopSkin_EventSelected;
        }

        private void Start()
        {
            _objUnlock.GetComponent<Button>().onClick.AddListener(BtnUnlock_OnClick);
            _objEquip.GetComponent<Button>().onClick.AddListener(BtnEquip_OnClick);
        }

        private void ShopSkin_EventSelected(CharacterSkinConfig config)
        {
            Setup(config);
        }

        private void Setup(CharacterSkinConfig config)
        {
            _config = config;

            _imgIcon.sprite = config.thumbnail;
            _imgIcon.preserveAspect = true;

            _objUnlock.SetActive(!config.data.isUnlocked);
            _objEquipped.SetActive(config.IsCurrent());
            _objEquip.SetActive(config.data.isUnlocked && !config.IsCurrent());
        }

        private void BtnUnlock_OnClick()
        {
            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                _config.data.Unlock();

                DataCharacterSkin.current = _config.name;
                DataCharacterSkin.Save();

                Setup(_config);

                _shopSkin.ForceRefresh();
            }, AdsPlacement.Shop_Skin_Unlock);
        }

        private void BtnEquip_OnClick()
        {
            DataCharacterSkin.current = _config.name;

            Setup(_config);

            _shopSkin.ForceRefresh();
        }
    }
}
