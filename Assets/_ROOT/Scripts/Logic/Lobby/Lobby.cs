using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Lobby : MonoBehaviour
    {
        private Lobby_Master _master;

        [Title("Reference")]
        [SerializeField] Player _player;
        [SerializeField] private AssetReferenceGameObject _viewGUI;
        [SerializeField] private float _prepareTime;
        [SerializeField] Transform _piggyPos;

        private Lobby_GUI _gui;

        private void Start()
        {
            ConstructStart().Forget();
        }
        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<Lobby_GUI>();

            if (_piggyPos != null) _player.cameraManager.cameraTutorial.Play(_piggyPos);

            StartCoroutine(PrepareStart());
        }
        IEnumerator PrepareStart()
        {
            yield return new WaitForSeconds(5);

            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _gui.announcement.PushMesseage($"Game will be selected in {Mathf.CeilToInt(currentTime)} second").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }

            _gui.announcement.PushMesseage($"Game start !!!").Forget();

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(DataMainGame.levelIndex +2);
        }
    }
}
