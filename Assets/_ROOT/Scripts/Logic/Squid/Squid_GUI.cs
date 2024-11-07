using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Squid_GUI : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Announcement _announcement;
        [SerializeField] private GameObject _weaponChoose;

        public Announcement announcement { get { return _announcement; } }
        public GameObject weaponChoose { get { return _weaponChoose; } }

        public void ChooseOption(int id)
        {
            StaticBus<Event_Squid_ChooseWeapon>.Post(new Event_Squid_ChooseWeapon(id));
            _weaponChoose.SetActive(false);
        }

    }
}
