using Cysharp.Threading.Tasks;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ColorBlock_ColorNotify : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private TextMeshProUGUI _txtColorName;
        [SerializeField] private TextMeshProUGUI _txtCountdown;
        [SerializeField] private Image _imgColor;

        private Tween _tween;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void Start()
        {
            gameObjectCached.SetActive(false);
        }

        public async UniTask Countdown(ColorBlock_ColorConfig config, float duration)
        {
            gameObjectCached.SetActive(true);

            _txtColorName.text = config.name;

            _imgColor.color = config.color;

            while (duration > 0f)
            {
                UpdateCountdown(duration);
                await UniTask.WaitForSeconds(0.1f, cancellationToken: this.GetCancellationTokenOnDestroy());
                duration -= 0.1f;
            }

            gameObjectCached.SetActive(false);
        }

        private void UpdateCountdown(float timeRemain)
        {
            _txtCountdown.text = timeRemain.ToString("0.0");
        }
    }
}
