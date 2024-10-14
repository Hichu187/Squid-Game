using UnityEngine;
using EasingCore;
using FancyScrollView;
using Sirenix.OdinInspector;
using System;

namespace Game
{
    class ShopSkin : FancyGridView<ShopSkinCellData, ShopSkinContext>
    {
        class CellGroup : DefaultCellGroup { }

        [Title("Config")]
        [SerializeField] private ShopSkinCell _prefab = default;

        public event Action<CharacterSkinConfig> eventSelected;

        private void Start()
        {
            Context.onCellSelected += (i) =>
            {
                ScrollTo(i, 0.3f, Ease.InOutSine);
            };

            ShopSkinCellData[] datas = new ShopSkinCellData[FactoryCharacter.skins.Length];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new ShopSkinCellData(FactoryCharacter.skins[i]);
            }

            UpdateContents(datas);
            UpdateSelection(0);
        }

        protected override void SetupCellTemplate()
        {
            Setup<CellGroup>(_prefab);
        }

        public void ForceRefresh()
        {
            Refresh();
        }

        public void UpdateSelection(int index)
        {
            if (Context.selectedIndex == index)
                return;

            Context.selectedIndex = index;
            Refresh();

            eventSelected?.Invoke(FactoryCharacter.skins[index]);
        }

        public void ScrollTo(int index, float duration, Ease easing)
        {
            UpdateSelection(index);
            ScrollTo(index, duration, easing, 0.5f);
        }

        public void JumpTo(int index)
        {
            UpdateSelection(index);
            JumpTo(index, 0.5f);
        }
    }
}
