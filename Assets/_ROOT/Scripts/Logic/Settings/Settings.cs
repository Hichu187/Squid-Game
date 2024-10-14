using LFramework;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class Settings : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private RectTransform _panel;
        [SerializeField] private GameObject _objHome;

        private void Start()
        {
            bool isHome = SceneManager.GetActiveScene().buildIndex == GameConstants.sceneHome;

            _objHome.SetActive(!isHome);
            _panel.rect.SetHeight(isHome ? 385f : 495f);

            _objHome.GetComponent<Button>().onClick.AddListener(BtnHome_OnClick);
        }

        private void BtnHome_OnClick()
        {
            SceneLoaderHelper.Load(GameConstants.sceneHome);
        }
    }
}
