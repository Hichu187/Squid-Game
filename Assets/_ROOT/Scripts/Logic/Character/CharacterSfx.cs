using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CharacterSfx : MonoCached
    {
        [Title("Config")]
        [SerializeField] private AudioConfig _sfxFootsteps;
        [SerializeField] private AudioConfig _sfxJump;

        [SerializeField] private float _footstepThreshold = 0.5f;

        private CharacterKC _kinematicController;

        private AudioScript _asFootsteps;

        private void Start()
        {
            _asFootsteps = AudioManager.Play(_sfxFootsteps, true);
            _asFootsteps.audioSource.enabled = false;

            _kinematicController = GetComponent<CharacterKC>();
        }

        private void Update()
        {
            _asFootsteps.transformCached.position = transformCached.position;

            if (_kinematicController.motor.GroundingStatus.IsStableOnGround)
                _asFootsteps.audioSource.enabled = _kinematicController.motor.BaseVelocity.magnitude > _footstepThreshold;
            else
                _asFootsteps.audioSource.enabled = false;
        }

        private void OnDestroy()
        {
            if (_asFootsteps != null)
            {
                _asFootsteps.audioSource.enabled = true;
                _asFootsteps.Stop();
            }
        }

        public void PlayJump()
        {
            AudioManager.Play(_sfxJump).transformCached.position = transformCached.position;
        }
    }
}
