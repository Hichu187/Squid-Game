using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RedLightGreenLight_Sfx_Manager : MonoBehaviour
    {
        [SerializeField] AudioSource source;
        [SerializeField] AudioClip clip;

        private void Awake()
        {
            StaticBus<Event_RedLightGreenLight_GreenLight>.Subscribe(CountDown);
        }
        private void OnDestroy()
        {
            StaticBus<Event_RedLightGreenLight_GreenLight>.Unsubscribe(CountDown);
        }

        private void CountDown(Event_RedLightGreenLight_GreenLight e)
        {
            float originalDuration = clip.length;

            float newPitch = originalDuration / e.countDown;

            source.pitch = newPitch;

            source.PlayOneShot(clip);
        }
    }
}
