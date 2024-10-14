using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TheFloorIsLava_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;

        public Announcement announcement { get { return _announcement; } }
    }
}
