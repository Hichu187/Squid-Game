using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MarbleShooting_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;

        public Announcement announcement { get { return _announcement; } }

    }
}
