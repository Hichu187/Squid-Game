using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TowerOfHell_Checkpoint : MonoBehaviour, ICharacterCollidable
    {
        [Title("Config")]
        [SerializeField] private GameObject _vfx;
        [SerializeField] private AudioConfig _sfx;

        private int _index;

        public void SetIndex(int index)
        {
            _index = index;
        }

        void ICharacterCollidable.OnCollisionEnter(Character character)
        {
            if (!character.isPlayer)
                return;

            if (_index > DataTowerOfHell.checkpointIndex.value || DataTowerOfHell.checkpointPenalty)
            {
                DataTowerOfHell.checkpointIndex.value = _index;

                _vfx.Create(transform.position, transform.rotation);

                AudioManager.Play(_sfx);

                UINotificationText.Push("New checkpoint reached!");

                DataTowerOfHell.checkpointPenalty = false;

                AdsHelper.ShowInterstitialBreak(AdsPlacement.TowerOfHell_Checkpoint);

                LogHelper.TowerOfHell_Checkpoint(_index);
            }

            LDebug.Log<TowerOfHell_Checkpoint>($"Checkpoint reached: {_index}");
        }

        void ICharacterCollidable.OnTriggerEnter(Character character)
        {
        }

        void ICharacterCollidable.OnTriggerExit(Character character)
        {
        }
    }
}
