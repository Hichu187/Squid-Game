using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CharacterFallDetector : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private float _threshold = 10f;

        private Character _character;

        private float _groundY;

        private void Awake()
        {
            _character = GetComponent<Character>();

            _groundY = _character.transformCached.position.y;

            _character.eventRevive += Character_EventRevive;
        }

        private void OnDestroy()
        {
            _character.eventRevive -= Character_EventRevive;
        }

        private void Update()
        {
            if (_character.motor.GroundingStatus.IsStableOnGround || _character.controller.stateCurrent == CharacterKC.State.Climbing)
                _groundY = _character.transformCached.position.y;
            else if (_groundY - _character.transformCached.position.y > _threshold)
                _character.Kill();
        }

        private void Character_EventRevive()
        {
            _groundY = _character.transformCached.position.y;
        }
    }
}
