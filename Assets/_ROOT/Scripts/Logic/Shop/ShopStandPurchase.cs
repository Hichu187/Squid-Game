using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShopStandPurchase : MonoBehaviour, ICharacterCollidable
    {
        [Title("Reference")]
        [SerializeField] private Image _imgConfirmProgress;

        [Title("Config")]
        [SerializeField] private float _confirmDuration;

        private Tween _tween;

        public event Action eventConfirm;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void Start()
        {
            _imgConfirmProgress.fillAmount = 0f;
        }

        private void InitTween()
        {
            if (_tween != null)
                return;

            _tween = _imgConfirmProgress.DOFillAmount(1f, _confirmDuration)
                                        .SetEase(Ease.Linear)
                                        .ChangeStartValue(0f);

            _tween.SetAutoKill(false);
            _tween.onComplete += () => { eventConfirm?.Invoke(); };
        }

        public void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        #region ICharacterCollidable

        void ICharacterCollidable.OnCollisionEnter(Character character)
        {
        }

        void ICharacterCollidable.OnTriggerEnter(Character character)
        {
            InitTween();

            _tween.PlayForward();
        }

        void ICharacterCollidable.OnTriggerExit(Character character)
        {
            InitTween();

            _tween.PlayBackwards();
        }

        #endregion
    }
}
