using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ParkourRace_GUI_Progress_Item : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Image _imgAvatar;

        private ParkourRace_Character _character;

        private RectTransform _rectTransform;

        public ParkourRace_Character character { get { return _character; } }

        public RectTransform rectTransform { get { return _rectTransform; } }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Construct(ParkourRace_Character character)
        {
            _character = character;

            _imgAvatar.sprite = character.character.rendererComp.skinConfig.avatar;
        }
    }
}
