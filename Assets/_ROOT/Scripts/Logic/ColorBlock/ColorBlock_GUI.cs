using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ColorBlock_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private ColorBlock_ColorNotify _colorNotify;

        public Announcement announcement { get { return _announcement; } }
        public ColorBlock_ColorNotify colorNotify { get {  return _colorNotify; } }
    }
}
