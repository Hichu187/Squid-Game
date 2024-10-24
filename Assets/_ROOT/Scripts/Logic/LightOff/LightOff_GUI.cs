using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LightOff_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private Image _fade;
        [SerializeField] private Announcement _gameTime;

        public Announcement announcement { get { return _announcement; } }
        public Announcement gameTime { get { return _gameTime; } }

        public void Fade()
        {
            _fade.gameObject.SetActive(true);
            _fade.DOFade(1, 0.5f);

            _fade.DOFade(0, 1f).SetDelay(2f).OnComplete(() =>
            {
                _fade.gameObject.SetActive(false);
            });
        }
    }
}
