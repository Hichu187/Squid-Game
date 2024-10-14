

namespace Game
{
    public static class LogHelper
    {
        public static void EnterMiniGame(MiniGameConfig config)
        {
            //BounceSdk.LogEvent("enter_minigame", "minigame_title", config.title);
        }

        public static void ExitMiniGame(MiniGameConfig config)
        {
            //BounceSdk.LogEvent("exit_minigame", "minigame_title", config.title);
        }

        public static void EasyObby_Checkpoint(int index)
        {
            //BounceSdk.LogEvent("easyobby_checkpoint", "checkpoint_index", index + 1, "minigame_title", FactoryMiniGame.easyObby.title);
        }

        public static void TheFloorIsLava_Start(TheFloorIsLava_LevelConfig config)
        {
            //BounceSdk.LogEvent("thefloorislava_start", "map", config.title, "minigame_title", FactoryMiniGame.theFloorIsLava.title);
        }

        public static void TheFloorIsLava_Win(TheFloorIsLava_LevelConfig config)
        {
            //BounceSdk.LogEvent("thefloorislava_win", "map", config.title, "minigame_title", FactoryMiniGame.theFloorIsLava.title);
        }

        public static void TowerOfHell_Checkpoint(int index)
        {
            //BounceSdk.LogEvent("towerofhell_checkpoint", "checkpoint_index", index + 1);
        }

        public static void BladeBall_Start(int index)
        {
            //BounceSdk.LogEvent("bladeball_start", "level", index + 1, "minigame_title", FactoryMiniGame.bladeBall.title);
        }
        public static void BladeBall_Win(int index)
        {
            //BounceSdk.LogEvent("bladeball_win", "level", index, "minigame_title", FactoryMiniGame.bladeBall.title);
        }

        public static void ParkourRace_Start(int index)
        {
            //BounceSdk.LogEvent("parkourrace_start", "level", index + 1);
        }

        public static void ParkourRace_Win(int index)
        {
            //BounceSdk.LogEvent("parkourrace_win", "level", index + 1);
        }
    }
}
