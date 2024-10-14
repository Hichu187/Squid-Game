using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class CharacterSkinData
    {
        [SerializeField] private bool _isUnlocked;

        public bool isUnlocked { get { return _isUnlocked; } }

        public void Unlock()
        {
            _isUnlocked = true;
        }
    }
}
