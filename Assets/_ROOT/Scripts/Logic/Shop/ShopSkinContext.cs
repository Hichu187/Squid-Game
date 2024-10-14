using FancyScrollView;
using System;

namespace Game
{
    class ShopSkinContext : FancyGridViewContext
    {
        public int selectedIndex = -1;

        public Action<int> onCellSelected;
    }
}
