using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BladeBall_Tutorial : MonoBehaviour
    {
        [SerializeField] Transform _ball;
        [SerializeField] BladeBall_Player _player;
        [SerializeField] Transform _canvas;
        [SerializeField] UIPointerClick _click;

        private void Awake()
        {
            _click.eventDown += Block;
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (!DataBladeBall.tutorialCompleted)
            {
                if (Vector3.Distance(_ball.position, _player.transform.position) <= _player.atkRange && Time.timeScale == 1)
                {
                    StartCoroutine(ReduceTimeScaleOverTime(0.05f));
                    _canvas.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator ReduceTimeScaleOverTime(float duration)
        {
            float startValue = Time.timeScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(startValue, 0f, elapsed / duration);
                yield return null;
            }

            Time.timeScale = 0f;
        }
        void Block()
        {
            _player.Block();
            DataBladeBall.tutorialCompleted = true;
            StopCoroutine(ReduceTimeScaleOverTime(0f));
            Time.timeScale = 1;

            _canvas.gameObject.SetActive(false);
        }
    }
}
