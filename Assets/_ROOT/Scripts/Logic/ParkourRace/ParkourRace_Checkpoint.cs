using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ParkourRace_Checkpoint : MonoCached, ICharacterCollidable
    {
        [Title("Config")]
        [SerializeField] private GameObject _vfx;
        [SerializeField] private AudioConfig _sfx;

        private int _index;

        public int index { get { return _index; } }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void PlayFX()
        {
            _vfx.Create(transform.position, transform.rotation);

            AudioManager.Play(_sfx);
        }

        void ICharacterCollidable.OnCollisionEnter(Character character)
        {
            StaticBus<Event_ParkourRace_Checkpoint>.Post(new Event_ParkourRace_Checkpoint(this, character));
        }

        void ICharacterCollidable.OnTriggerEnter(Character character)
        {
        }

        void ICharacterCollidable.OnTriggerExit(Character character)
        {
        }
    }
}
