using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class FactoryPrefab : ScriptableObjectSingleton<FactoryPrefab>
    {
        [Title("Ads")]
        [SerializeField] AssetReference _adsBreakPopup;

        [Title("Confirm")]
        [SerializeField] GameObject _confirmPopup;

        [Title("Debug")]
        [SerializeField] AssetReference _debugView;

        [Title("Game")]
        [SerializeField] GameObject _gameInit;

        [Title("UI")]
        [SerializeField] GameObject _uiNotificationText;

        public static AssetReference adsBreakPopup => instance._adsBreakPopup;

        public static GameObject confirmPopup => instance._confirmPopup;

        public static AssetReference debugView => instance._debugView;

        public static GameObject gameInit => instance._gameInit;

        public static GameObject uiNotificationText => instance._uiNotificationText;
    }
}