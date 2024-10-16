using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TugOfWar_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private UIPointerClick _pull;

        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _hook;
        [SerializeField] private Image _progress;
        public Announcement announcement { get { return _announcement; } }
        public UIPointerClick pull { get { return _pull; } }
        public RectTransform target { get { return _target; } }
        public RectTransform hook { get { return _hook; } }
        public Image progress { get { return _progress; } }

        private void Start()
        {
            _progress.fillAmount = 0.5f;
        }

    }
}
