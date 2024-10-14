using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CharacterDie : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private AudioConfig _sfx;

        private GameObject _objRagdoll;

        private GameObject _objRootCached;

        private GameObject _objRoot
        {
            get
            {
                if (_objRootCached == null && transform.childCount > 1)
                    _objRootCached = transform.GetChild(1).gameObject;
                return _objRootCached;
            }
        }

        private void Start()
        {
            Character character = GetComponent<Character>();

            character.eventDie += Character_EventDie;
            character.eventRevive += Character_EventRevive;
        }

        private void Character_EventRevive()
        {
            if (_objRagdoll != null)
            {
                Destroy(_objRagdoll);
                _objRagdoll = null;
            }

            if (_objRoot != null)
                _objRoot.SetActive(true);
        }

        private void Character_EventDie()
        {
            if (_objRoot != null)
            {
                _objRagdoll = _objRoot.Create(_objRoot.transform.parent);

                _objRoot.SetActive(false);

                _objRagdoll.GetComponentInChildren<CharacterRagdoll>().Explode();
            }

            AudioManager.Play(_sfx).transformCached.position = transform.position;
        }
    }
}
