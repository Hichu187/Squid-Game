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

        [Title("Config AI")]
        [SerializeField] private GameObject _prefab;
        public int count = 10;
        [SerializeField] private Vector2 _idleDurationRange = new Vector2(0f, 0.5f);
        [SerializeField] private float _randomPositionRadius;
        [SerializeField] Transform gate;

        private AI[] _ai;

        private void Start()
        {
            ConstructStart().Forget();
        }
        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);

            _gui = view.GetComponent<Lobby_GUI>();

            if (_piggyPos != null) _player.cameraManager.cameraTutorial.Play(_piggyPos);

            if(DataMainGame.levelIndex >= 6)
            {
                StartCoroutine(FinalWin());
            }
            else
            {
                StartCoroutine(PrepareStart());
            }
            SpawnAI();

        }
        IEnumerator PrepareStart()
        {
            yield return new WaitForSeconds(5);

            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _gui.announcement.PushMesseage($"Move to the gate to reach the next game !!!").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }

            _gui.announcement.PushMesseage($"Move to the gate to reach the next game !!!").Forget();

            yield return new WaitForSeconds(1);

            foreach (var ai in _ai)
            {
                ai.Chase(gate);
            }


        }

        IEnumerator FinalWin()
        {
            yield return new WaitForSeconds(5);

            float currentTime = _prepareTime-2;

            while (currentTime > 0)
            {
                _gui.announcement.PushMesseage($"YOU WIN").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }

            _player.character.animator.PlayWin();

            yield return new WaitForSeconds(1);

            SceneLoaderHelper.Load(0);
        }

        private void SpawnAI()
        {
            _ai = new AI[count];

            for (int i = 0; i < count; i++)
            {
                AI ai = _prefab.Create().GetComponent<AI>();

                ai.gameObject.AddComponent<AIFollowWaypoint>();

                Vector3 position = Random.insideUnitSphere * _randomPositionRadius;

                position.y = 0f;

                ai.character.Revive(position, Quaternion.LookRotation(Vector3.forward, Vector3.up));

                Character character = ai.transform.GetChild(0).GetComponent<Character>();

                float idleDuration = _idleDurationRange.RandomWithin();

                ai.SetIdleDurationRange(new Vector2(idleDuration, idleDuration));

                _ai[i] = ai;
            }
        }

        public void LoadGameScene()
        {
            SceneLoaderHelper.Load(DataMainGame.levelIndex + 2);
        }
    }
}
