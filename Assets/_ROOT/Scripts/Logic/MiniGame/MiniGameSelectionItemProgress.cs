using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class MiniGameSelectionItemProgress : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objProgressIncomplete;
        [SerializeField] private GameObject _objProgressComplete;

        public void Construct(float progress, int decimalPlace = 0)
        {
            progress = Mathf.Clamp01(progress);

            _objProgressComplete.SetActive(progress >= 1f);
            _objProgressIncomplete.SetActive(progress < 1f);

            UIProgressBar progressBar = _objProgressIncomplete.GetComponent<UIProgressBar>();

            progressBar.UpdateProgress(progress, decimalPlace);
        }

        public void Hide()
        {
            _objProgressComplete.SetActive(false);
            _objProgressIncomplete.SetActive(false);
        }
    }
}
