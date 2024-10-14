using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIProgressBar : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public void UpdateProgress(float progress, int decimalPlaceCount = 0)
        {
            _image.fillAmount = progress;

            progress *= 100f;

            float a = Mathf.Pow(10f, decimalPlaceCount);

            _text.text = $"Progress: {Mathf.Round(progress * a) / a}%";
        }
    }
}
