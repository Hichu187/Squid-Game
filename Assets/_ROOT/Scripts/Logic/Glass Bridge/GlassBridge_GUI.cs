using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private Announcement _gameTime;

        public Announcement announcement { get { return _announcement; } }
        public Announcement gameTime { get { return _gameTime; } }
    }
}
