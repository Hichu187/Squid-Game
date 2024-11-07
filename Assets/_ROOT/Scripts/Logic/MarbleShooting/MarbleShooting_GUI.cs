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
        [SerializeField] private MarbleShooting_Count _count;

        public Announcement announcement { get { return _announcement; } }
        public MarbleShooting_Count count { get { return _count; } }

        private void OnEnable()
        {
            count.gameObject.SetActive(false);
        }

    }
}
