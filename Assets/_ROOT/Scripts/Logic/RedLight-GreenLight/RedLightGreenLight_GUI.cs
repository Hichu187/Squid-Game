using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RedLightGreenLight_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private Announcement _gameTime;
        [SerializeField] private LightSwitch _notice;

        public Announcement announcement { get { return _announcement; } }
        public Announcement gameTime { get { return _gameTime; } }
        public LightSwitch notice { get { return _notice; } }
    }
}
