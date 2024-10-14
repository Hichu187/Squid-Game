namespace Game
{
    public static class TowerOfHell_Helper 
    {
        public static float GetProgress()
        {
            int current = DataTowerOfHell.checkpointIndex.value;
            int max = FactoryTowerOfHell.checkpointCount;

            return (float) (current / max);
        }
    }
}
