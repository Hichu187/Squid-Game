using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TowerOfHell_Revive_Countdown : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private TextMeshProUGUI _txtCountdown;
        [SerializeField] private Image _imgCountdown;

        [Title("Config")]
        [SerializeField] private float _delay;
        [SerializeField] private int _duration;

        private Tween _tween;

        private View _view;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void Start()
        {
            _view = GetComponent<View>();

            _tween?.Kill();

            _imgCountdown.fillAmount = 1f;
            _txtCountdown.text = _duration.ToString();

            Sequence sequence = DOTween.Sequence();

            sequence.SetDelay(_delay);
            sequence.Append(DOVirtual.Int(_duration, 0, _duration, (x) => { _txtCountdown.text = x.ToString(); }));
            sequence.Join(_imgCountdown.DOFillAmount(0f, _duration).ChangeStartValue(1f));
            sequence.AppendCallback(_view.Close);
            sequence.SetUpdate(true);

            _tween = sequence;
        }

        public void Stop()
        {
            _tween?.Kill();
        }
    }
}
