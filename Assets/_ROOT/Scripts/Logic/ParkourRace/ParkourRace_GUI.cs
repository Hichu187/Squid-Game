using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ParkourRace_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private Announcement_Coundown _announcementCountdown;
        [SerializeField] private ParkourRace_GUI_Progress _progress;

        public Announcement_Coundown announcementCountdown { get { return _announcementCountdown; } }

        public ParkourRace_GUI_Progress progress { get { return _progress; } }

        private void Start()
        {
            _txtLevel.text = $"Level {DataParkourRace.levelIndex + 1}";
        }
    }
}
