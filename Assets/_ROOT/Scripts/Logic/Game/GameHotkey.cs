#if UNITY_EDITOR

using LFramework;
using System;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public static class GameHotKey
    {
        [RuntimeInitializeOnLoadMethod()]
        static void RuntimeInit()
        {
            MonoCallback.Instance.EventUpdate += MonoCallback_EventUpdate;
        }

        static void MonoCallback_EventUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneLoaderHelper.Reload();
            }
            else if (Input.GetKeyDown(KeyCode.Print))
            {
                CaptureScreenshot();
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                ViewHelper.PushAsync(FactoryPrefab.debugView);
            }
        }

        [MenuItem("Game/Capture Screenshot")]
        private static void CaptureScreenshot()
        {
            var currentTime = DateTime.Now;
            var filename = currentTime.ToString().Replace('/', '-').Replace(':', '_') + ".png";
            var path = "Assets/" + filename;

            ScreenCapture.CaptureScreenshot(path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

#endif