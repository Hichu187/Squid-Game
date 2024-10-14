#define USE_VIBRATION

using LFramework;
using UnityEngine;

namespace Game
{
    public class GameInit : MonoSingleton<GameInit>
    {
        [RuntimeInitializeOnLoadMethod]
        private static void StartupInit()
        {
            FactoryPrefab.gameInit.Create();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            InitSettings();
            InitSRDebug();
            InitRemoveAds();
        }

        #region Ads

        private static void InitRemoveAds()
        {

        }

        #endregion

        #region Settings

        private void InitSettings()
        {
            AudioManager.volumeMusic.value = DataSettings.musicVolume.value;
            AudioManager.volumeSound.value = DataSettings.soundVolume.value;

            DataSettings.musicVolume.eventValueChanged += (volume) => { AudioManager.volumeMusic.value = volume; };
            DataSettings.soundVolume.eventValueChanged += (volume) => { AudioManager.volumeSound.value = volume; };

#if USE_VIBRATION
            //Taptic.Taptic.tapticOn = DataSettings.vibration.value;

            DataSettings.vibration.eventValueChanged += SettingsVibrationValue_EventValueChanged;
#endif
        }

        private void SettingsVibrationValue_EventValueChanged(bool isOn)
        {
#if USE_VIBRATION
            //Taptic.Taptic.tapticOn = isOn;
#endif
        }

        #endregion

        #region SRDebug

        private void InitSRDebug()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD



#endif
        }

        #endregion
    }
}
