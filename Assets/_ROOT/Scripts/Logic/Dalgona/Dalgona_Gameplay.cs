using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Gameplay : MonoBehaviour
    {
        private Dalgona_Master _master;

        [Title("Reference")]
        public List<GameObject> candy;
        [SerializeField] private float _gameTime;
        [SerializeField] private float _prepareTime;

        [SerializeField] int option;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] bool _isStart;
        [SerializeField] bool _isStop;
        [SerializeField] bool _isWin;
        [SerializeField] Transform _needle;
        [SerializeField] Transform _prepare;
        [SerializeField] Transform _gameplay;
        private Vector3 offset;
        private bool isDragging = false;

        private void Awake()
        {
            StaticBus<Event_Dalgona_Constructed>.Subscribe(Constructed);
            StaticBus<Event_Dalgona_RoundStart>.Subscribe(RoundStart);
            StaticBus<Event_Dalgona_Lose>.Subscribe(Lose);
            StaticBus<Event_Dalgona_Win>.Subscribe(Win);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Dalgona_Constructed>.Unsubscribe(Constructed);
            StaticBus<Event_Dalgona_RoundStart>.Unsubscribe(RoundStart);
            StaticBus<Event_Dalgona_Win>.Unsubscribe(Win);
            StaticBus<Event_Dalgona_Lose>.Unsubscribe(Lose);
        }
        void Start()
        {
            _master = GetComponent<Dalgona_Master>();
            _mainCamera = _master.mainCamera;
        }


        void Constructed(Event_Dalgona_Constructed e)
        {
            StartCoroutine(PrepareStart());
        }

        public void RoundStart(Event_Dalgona_RoundStart e)
        {

        }

        IEnumerator PrepareStart()
        {
            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _master.gui.announcement.PushMesseage($"Choose  1  in  4 ").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }

            SetupOption();

            _master.gui.announcement.PushMesseage($"Your Option is {option} ").Forget();

            yield return new WaitForSeconds(2);

            _master.gui.Fade();

            yield return new WaitForSeconds(0.5f);

            StaticBus<Event_Dalgona_RoundStart>.Post(null);

            _master.player.gui.gameObject.SetActive(false);

            _prepare.gameObject.SetActive(false);

            yield return new WaitForSeconds(1);

            OptionPrepare();
        }

        void SetupOption()
        {
            if (option == 0) option = Random.Range(1, 5);

        }

        void OptionPrepare()
        {
            _gameplay.gameObject.SetActive(true);
            _master.player.character.gameObject.SetActive(false);
            _isStart = true;

            GameObject c = Instantiate(candy[Random.Range(0, candy.Count)], _gameplay.transform);
            StartCoroutine(GameTimeCountDown());
        }
        IEnumerator GameTimeCountDown()
        {
            float currentTime = _gameTime;

            while (currentTime > 0)
            {
                _master.gui.gameTime.PushMesseage($"{Mathf.CeilToInt(currentTime)}").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }
            _master.gui.gameTime.PushMesseage($"Finish !!!").Forget();

            if (!_isWin)
            {
                _isStop = true;
                StopAllCoroutines();
                StartCoroutine(LoseSetup());
            }

        }

        void Update()
        {
            if (_isStart && !_isStop)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = touch.position;

                    float zDepth = 5f;
                    Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, zDepth));

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (!isDragging)
                            _needle.transform.position += Vector3.down * 0.1f;

                        offset = _needle.transform.position - worldPosition;
                        isDragging = true;
                    }

                    if (touch.phase == TouchPhase.Moved && isDragging)
                    {
                        Vector3 targetPosition = new Vector3(worldPosition.x + offset.x, worldPosition.y + offset.y, worldPosition.z + offset.z);
                        _needle.transform.position = Vector3.Lerp(_needle.transform.position, targetPosition, 0.2f); // Di chuyển mượt với Lerp
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (isDragging)
                            _needle.transform.position += Vector3.up * 0.1f;

                        isDragging = false;
                    }
                }
            }
        }

        void Lose(Event_Dalgona_Lose e)
        {
            _isStop = true;
            StopAllCoroutines();
            StartCoroutine(LoseSetup());
        }
        IEnumerator LoseSetup()
        {
            yield return new WaitForSeconds(2);
            _gameplay.gameObject.SetActive(false);
            _master.gui.Fade();
            _master.player.character.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            _master.player.gui.gameObject.SetActive(true);
            //_prepare.gameObject.SetActive(true);


            GetComponent<Dalgona_Camera>().ResetCamera();

            yield return new WaitForSeconds(1);
            _master.player.GetComponent<Dalgona_Player>().GetTarget();
            yield return new WaitForSeconds(1);
            _master.SpawnResultLose().Forget();

        }
        void Win(Event_Dalgona_Win e)
        {
            _isStop = true;
            _isWin = true;
            StopAllCoroutines();
            StartCoroutine(WinSetup());

            DataMainGame.levelIndex++;
        }
        
        IEnumerator WinSetup()
        {
/*            yield return new WaitForSeconds(2);

            _master.gui.Fade();

            yield return new WaitForSeconds(0.5f);

            _master.player.gui.gameObject.SetActive(true);
            _prepare.gameObject.SetActive(true);
            _gameplay.gameObject.SetActive(false);

            GetComponent<Dalgona_Camera>().ResetCamera();*/

            yield return new WaitForSeconds(2f);

            _master.SpawnResultView().Forget();
        }
    }
}
