using FancyScrollView;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    class ShopSkinCell : FancyGridViewCell<ShopSkinCellData, ShopSkinContext>
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objEquippedBG;
        [SerializeField] private GameObject _objEquipped;
        [SerializeField] private GameObject _objNormalBG;
        [SerializeField] private GameObject _objLocked;
        [SerializeField] private GameObject _objFocus;

        [SerializeField] private Image _imgIcon;

        public override void Initialize()
        {
            GetComponent<Button>().onClick.AddListener(() => Context.onCellSelected?.Invoke(Index));
        }

        public override void UpdateContent(ShopSkinCellData data)
        {
            var selected = Context.selectedIndex == Index;

            _objEquipped.SetActive(data.config.IsCurrent());
            _objEquippedBG.SetActive(data.config.IsCurrent());

            _objNormalBG.SetActive(!_objEquippedBG.activeSelf);

            _objLocked.SetActive(!data.config.data.isUnlocked);

            _objFocus.SetActive(selected);

            _imgIcon.sprite = data.config.thumbnail;
            _imgIcon.preserveAspect = true;
        }
    }
}
