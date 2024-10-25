using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class BladeBall_Manager : MonoBehaviour
    {
        [SerializeField] int levelIndex;
        [SerializeField] Player _player;
        [SerializeField] BladeBall_Ball _ball;
        [SerializeField] Transform _startPos;
        [SerializeField] TextMeshProUGUI _notice;
        [SerializeField] TextMeshProUGUI _enemCount;
        [SerializeField] TextMeshProUGUI _levelTxt;
        //[SerializeField] List<BladeBall_LevelConfig> _level;
        [SerializeField] Transform _root;
        [SerializeField] Transform _rootAvatar;
        [SerializeField] GameObject _avaPrefab;
        [SerializeField] GameObject weaponSelect;
        [SerializeField] CinemachineFreeLook _freeLook;
        [SerializeField] AudioConfig _soloSfx;
        public List<Transform> _enem;
        private int startEnem;
        [SerializeField] private AssetReferenceGameObject _viewGUI;

        private void Awake()
        {
            StaticBus<Event_Player_Die>.Subscribe(StaticBus_Player_Die);
            StaticBus<Event_BladeBall_AiDie>.Subscribe(AI_Character_Die);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Player_Die>.Unsubscribe(StaticBus_Player_Die);
            StaticBus<Event_BladeBall_AiDie>.Unsubscribe(AI_Character_Die);
        }

        private void Start()
        {
            levelIndex = DataBladeBall.levelIndex;
            _levelTxt.text = "Level  " + (levelIndex + 1);
            LevelLoadAsync(levelIndex).Forget();

            ConstructStart().Forget();

            GameInit();
        }

        /*
        private void LevelLoad(int level)
        {
            BladeBall_Map map = Instantiate(_level[level].prefab.GetComponent<BladeBall_Map>());
            _startPos = map.playerStartPosition;
            GetComponent<BladeBall_AI_Manager>()._map = map;
            StaticBus<Event_BladeBall_LevelConstructed>.Post(new Event_BladeBall_LevelConstructed(_level[level].listAi));
            startEnem = _enem.Count;
            _enemCount.text = _enem.Count + "/" + startEnem;
        }
        */

        private async UniTaskVoid LevelLoadAsync(int levelIndex)
        {
            Player.Instance.SetEnabled(false);

            //var handle = Addressables.LoadAssetAsync<GameObject>(FactoryBladeBall.levelConfigs[levelIndex].prefabAsset);

            //await handle;

            //Player.instance.SetEnabled(true);

            //BladeBall_Map map = handle.Result.Create().GetComponent<BladeBall_Map>();

            //handle.Release();

/*            _startPos = map.playerStartPosition;
            GetComponent<BladeBall_AI_Manager>()._map = map;
            StaticBus<Event_BladeBall_LevelConstructed>.Post(new Event_BladeBall_LevelConstructed(FactoryBladeBall.levelConfigs[levelIndex].listAi));
            startEnem = _enem.Count;
            _enemCount.text = _enem.Count + "/" + startEnem;

            _player.character.motor.SetPositionAndRotation(_startPos.position, Quaternion.LookRotation(-_startPos.position.normalized, Vector3.up));
            _freeLook.ForceCameraPosition(_player.character.transformCached.TransformPoint(new Vector3(0f, 7f, -10f)), _player.character.transformCached.rotation);*/
        }

        private void GameInit()
        {
            if (DataBladeBall.tutorialCompleted)
            {
                weaponSelect.SetActive(true);
            }
            else
            {
                StartGame();
            }
        }

        public void StartGame()
        {
            LogHelper.BladeBall_Start(DataBladeBall.levelIndex);
            StartCoroutine(StartGameAfterDelay(3));

            foreach (var e in _enem)
            {
                e.parent.GetComponent<AIFollowWaypoint>().enabled = true;
            }
        }
        private async UniTaskVoid ConstructStart()
        {
            View view = await ViewHelper.PushAsync(_viewGUI);
        }
        private void AI_Character_Die(Event_BladeBall_AiDie e)
        {
            _enem.Remove(e.character.transform);

            if (_enem.Count == 0)
            {
                _enemCount.text = "";

                DataBladeBall.levelIndex++;
                DataBladeBall.loseCount = 0;

                StaticBus<Event_BladeBall_Result>.Post(new Event_BladeBall_Result(true));

                LogHelper.BladeBall_Win(DataBladeBall.levelIndex);
            }
            else
            {
                _enemCount.text = _enem.Count + "/" + startEnem;
                StartCoroutine(Respawn(3));
            }

            if (_enem.Count == 1)
            {
                _enemCount.text = "";
                StartCoroutine(Notice(3f, "<color=#FF0000>SOLO</color>"));
                _notice.fontSize = 55;
                _notice.transform.DOScale(1.25f, 0.3f).SetLoops(10, LoopType.Yoyo);
                AudioManager.Play(_soloSfx, false);
            }
            else
            {
                _notice.fontSize = 36;
                _notice.transform.DOScale(1f, 0f);
                if (_ball.preTarget == _player.character.transform)
                {
                    int id = Random.Range(1000, 9999);
                    StartCoroutine(Notice(1.5f, $"You kill <color=#FF0000> Player{Mathf.CeilToInt(id)}</color>"));
                }
                else
                {
                    StartCoroutine(Notice(1.5f, "Player" + Random.Range(1000, 9999) + " kill Player" + Random.Range(1000, 9999)));
                }
            }
        }

        private void StaticBus_Player_Die(Event_Player_Die e)
        {
            StaticBus<Event_BladeBall_Result>.Post(new Event_BladeBall_Result(false));

            DataBladeBall.loseCount++;
        }

        private IEnumerator Respawn(float delay)
        {
            float countdown = delay;
            while (countdown > 0)
            {
                yield return new WaitForSeconds(1f);
                countdown -= 1f;
            }

            StaticBus<Event_BladeBall_LevelStart>.Post(null);
            yield return new WaitForSeconds(1f);

        }

        private IEnumerator StartGameAfterDelay(float delay)
        {
            float countdown = delay;
            while (countdown > 0)
            {
                _notice.text = $"Game ready in:  <color=#FF0000>{Mathf.CeilToInt(countdown)}</color>";
                yield return new WaitForSeconds(1f);
                countdown -= 1f;
            }

            _notice.text = "Start !";
            StaticBus<Event_BladeBall_LevelStart>.Post(null);

            yield return new WaitForSeconds(1f);
            _notice.text = "";
        }
        private IEnumerator Notice(float delay, string Text)
        {
            _notice.text = Text;
            yield return new WaitForSeconds(delay);
            _notice.text = "";
        }

    }
}
