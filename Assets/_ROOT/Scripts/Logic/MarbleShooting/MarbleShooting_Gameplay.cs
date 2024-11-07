using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.CharacterRagdoll;

namespace Game
{
    public class MarbleShooting_Gameplay : MonoBehaviour
    {
        private MarbleShooting_Master _master;
        [Title("Reference")]
        [SerializeField] GameObject _marblePrefab;
        [SerializeField] Transform _preparePos;
        [SerializeField] Transform _otherPos;
        [SerializeField] int wave;
        [SerializeField] int playerCount;
        [SerializeField] int otherCount;

        [Title("Config")]
        [SerializeField] private bool isStart;
        [SerializeField] private bool isPlayerTurn;
        [SerializeField] private float _prepareTime;
        [SerializeField] Rigidbody _curBall;
        [SerializeField] float throwForceMultiplier = 10f;
        [SerializeField] float arcAngle = 30f;
        [SerializeField] Material material;
        [SerializeField] List<MarbleShooting_Marble> _pmarbles;
        [SerializeField] List<MarbleShooting_Marble> _emarbles;

        private Vector2 startTouchPosition;
        private Vector2 endTouchPosition;
        private void Awake()
        {
            StaticBus<Event_MarbleShooting_Constructed>.Subscribe(Constructed);
            StaticBus<Event_MarbleShooting_ChangePhase>.Subscribe(ChangePhase);
            StaticBus<Event_MarbleShooting_Count>.Subscribe(Count);
        }

        private void OnDestroy()
        {
            StaticBus<Event_MarbleShooting_Constructed>.Unsubscribe(Constructed);
            StaticBus<Event_MarbleShooting_ChangePhase>.Unsubscribe(ChangePhase);
            StaticBus<Event_MarbleShooting_Count>.Unsubscribe(Count);
        }

        private void Start()
        {
            _master = GetComponent<MarbleShooting_Master>();
        }
        private void Update()
        {
            ThrowObject();
        }
        void Constructed(Event_MarbleShooting_Constructed e)
        {
            StartCoroutine(PrepareStart());
        }
        IEnumerator PrepareStart()
        {
            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _master.gui.announcement.PushMesseage($"Throw the marble into the hole to score points").Forget();
                currentTime -= Time.deltaTime;

                yield return null;
            }

            StartFighting();
        }
        void StartFighting()
        {
            _master.player.cameraManager.DisableCamera();

            isStart = true;

            StartCoroutine(PlayerPhaseSetUp(0));

            _master.gui.count.gameObject.SetActive(true);
        }
        void ChangePhase(Event_MarbleShooting_ChangePhase e)
        {
            if(wave > 0)
            {
                isPlayerTurn = !isPlayerTurn;
                if (isPlayerTurn)
                {
                    StartCoroutine(PlayerPhaseSetUp(0));
                }
                else
                {
                    StartCoroutine(OtherPlayerTurn());

                }
            }
            else
            {
                GameResult();
            }

        }
        IEnumerator PlayerPhaseSetUp(float t)
        {
            yield return new WaitForSeconds(t);

            _master.gui.announcement.PushMesseage($"Your Turn").Forget();
            yield return new WaitForSeconds(1f);

            GameObject marble = Instantiate(_marblePrefab, _preparePos);
            _curBall = marble.GetComponent<Rigidbody>();
            _curBall.GetComponent<MarbleShooting_Marble>().isPlayer = true;

            _pmarbles.Add(_curBall.GetComponent<MarbleShooting_Marble>());
        }
        IEnumerator OtherPlayerTurn()
        {
            _master.gui.announcement.PushMesseage($"Other Player's Turn").Forget();
            yield return new WaitForSeconds(1f);

            GameObject marble = Instantiate(_marblePrefab, _otherPos);
            _curBall = marble.GetComponent<Rigidbody>();
            marble.GetComponent<Renderer>().material = material;
            _curBall.GetComponent<MarbleShooting_Marble>().isPlayer = false;
            _emarbles.Add(_curBall.GetComponent<MarbleShooting_Marble>());

            float swipeDistance = 2f;

            float hitChance = 0f;

            switch (wave)
            {
                case 5:
                    hitChance = 0.2f;
                    break;
                case 4:
                    hitChance = 0.4f;
                    break;
                case 3:
                    hitChance = 0.6f;
                    break;
                case 2:
                    hitChance = 0.8f;
                    break;
                case 1:
                    hitChance = 0.8f;
                    break;
            }
            float randomValue = Random.value;
            float throwX = 0f;
            if (randomValue > hitChance)
            {
                throwX = Random.Range(-5f, 5f);
            }

            Vector3 throwDirection = new Vector3(throwX, 0f, -10).normalized;

            throwDirection = Quaternion.Euler(-arcAngle, 0f, 0f) * throwDirection;

            _curBall.isKinematic = false;
            _curBall.AddForce(throwDirection * swipeDistance * throwForceMultiplier*Random.Range(300,375), ForceMode.Impulse);

            //
            _master.gui.count.otherScore[5 - wave].color = Color.gray;
            wave--;
            StartCoroutine(marbleDelay());

        }
        void ThrowObject()
        {
            if (!isPlayerTurn || _curBall == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endTouchPosition = Input.mousePosition;
                Vector2 swipeDirection = endTouchPosition - startTouchPosition;
                float swipeDistance = swipeDirection.magnitude;

                Vector3 throwDirection = new Vector3(swipeDirection.x, 0f, swipeDirection.y).normalized;

                throwDirection = Quaternion.Euler(-arcAngle, 0f, 0f) * throwDirection;

                _curBall.isKinematic = false;
                _curBall.AddForce(throwDirection * swipeDistance * throwForceMultiplier, ForceMode.Impulse);

                //
                _master.gui.count.playerScore[5 - wave].color = Color.gray;
                StartCoroutine(marbleDelay());
            }
        }
        IEnumerator marbleDelay()
        {
            yield return new WaitForSeconds(0.15f);
            _curBall.GetComponent<MarbleShooting_Marble>().hasStopped = false;
            _curBall = null;
        }
        private void Count(Event_MarbleShooting_Count e)
        {
            if (e._isPlayer)
            {
                _master.gui.count.playerScore[_pmarbles.IndexOf(e._marble)].color = Color.green;
                playerCount++;
            }
            else
            {
                _master.gui.count.otherScore[_emarbles.IndexOf(e._marble)].color = Color.green;
                otherCount++;
            }
        }
        private void GameResult()
        {
            if (playerCount > otherCount)
            {
                StaticBus<Event_MarbleShooting_Win>.Post(null);
                _master.player.cameraManager.EnableCamera();
                _master.player.gui.gameObject.SetActive(true);
                _master.gui.announcement.PushMesseage($"You Win!!!").Forget();
                _master.SpawnResultView().Forget();
                DataMainGame.levelIndex++;
            }
            else if( playerCount < otherCount)
            {
                StaticBus<Event_MarbleShooting_Lose>.Post(null);
                _master.player.cameraManager.EnableCamera();
                _master.player.GetComponent<MarbleShooting_Player>().GetTarget();
                _master.gui.announcement.PushMesseage($"You Lose!!!").Forget();

                StartCoroutine(Lose());

            }
            else
            {
                StaticBus<Event_MarbleShooting_Draw>.Post(null);
                _master.gui.announcement.PushMesseage($"The match ended in a draw. Rematch!!!").Forget();

                ResetBattle();
            }
        }

        IEnumerator Lose()
        {
            yield return new WaitForSeconds(2f);
            _master.SpawnResultLose().Forget();
        }
        void ResetBattle()
        {
            wave = 5;
            playerCount = 0;
            otherCount = 0;

            isPlayerTurn = true;

            _pmarbles.Clear();
            _emarbles.Clear();

            foreach (Transform child in _preparePos)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _otherPos)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(PlayerPhaseSetUp(2));
        }

    }
}
