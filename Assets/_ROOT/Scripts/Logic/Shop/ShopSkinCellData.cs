namespace Game
{
    class ShopSkinCellData
    {
        public CharacterSkinConfig config { get; private set; }

        public ShopSkinCellData(CharacterSkinConfig config)
        {
            this.config = config;
        }
    }
}
