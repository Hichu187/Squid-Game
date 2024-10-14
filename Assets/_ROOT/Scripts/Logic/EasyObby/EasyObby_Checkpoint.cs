using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [SelectionBase]
    public class EasyObby_Checkpoint : MonoCached, ICharacterCollidable
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objTrigger;

        [Title("Config")]
        [SerializeField] private GameObject _vfx;
        [SerializeField] private AudioConfig _sfx;

        private int _index;

        public int index { get { return _index; } }

        public void Construct(int index)
        {
            _index = index;

            _objTrigger.SetActive(true);
        }

        public void PlayFX()
        {
            // Play vfx
            _vfx.Create(transformCached.position, transformCached.rotation);

            // Play sfx
            AudioManager.Play(_sfx);

            // Haptic
            //Taptic.Taptic.Medium();
        }

        public Vector3 GetRandomPosition()
        {
            float randomRange = 0.3f;

            float x = Random.Range(FactoryEasyObby.checkpointSize.x * -randomRange, FactoryEasyObby.checkpointSize.x * randomRange);
            float y = FactoryEasyObby.checkpointSize.y * 0.5f;
            float z = Random.Range(FactoryEasyObby.checkpointSize.z * -randomRange, FactoryEasyObby.checkpointSize.z * randomRange);

            return transformCached.TransformPoint(new Vector3(x, y, z));
        }

        void ICharacterCollidable.OnCollisionEnter(Character character)
        {
        }

        void ICharacterCollidable.OnTriggerEnter(Character character)
        {
            StaticBus<Event_EasyObby_Checkpoint>.Post(new Event_EasyObby_Checkpoint(this, character));
        }

        void ICharacterCollidable.OnTriggerExit(Character character)
        {
        }
    }
}
