namespace Game
{
    public static class TheFloorIsLava_Helper
    {
        public static float GetProgress()
        {
            int completedCount = 0;

            for (int i = 0; i < FactoryTheFloorIsLava.levelConfigs.Length; i++)
            {
                if (FactoryTheFloorIsLava.levelConfigs[i].data.isCompleted)
                    completedCount++;
            }

            return (float)completedCount / FactoryTheFloorIsLava.levelConfigs.Length;
        }
    }
}
