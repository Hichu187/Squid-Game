using LFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public static class TimeManager
    {
        [RuntimeInitializeOnLoadMethod]
        private static void InitOnStartup()
        {
            MonoCallback.Instance.EventActiveSceneChanged += Instance_EventActiveSceneChanged;
        }

        private static void Instance_EventActiveSceneChanged(Scene arg1, Scene arg2)
        {
            Time.timeScale = 1f;
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }
    }
}
