using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    [System.Serializable]
    public class CharacterSkinConfig : ScriptableObject
    {
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] private Sprite _avatar;
        [SerializeField] private AssetReferenceGameObject _prefab;

        [NonSerialized] private CharacterSkinData _data;

        public Sprite thumbnail { get { return _thumbnail; } }
        public Sprite avatar { get { return _avatar; } }
        public AssetReferenceGameObject prefab { get { return _prefab; } }

        public CharacterSkinData data { get { if (_data == null) _data = DataCharacterSkin.Get(name); return _data; } }

        public bool IsCurrent()
        {
            return string.CompareOrdinal(DataCharacterSkin.current, this.name) == 0;
        }
    }
}
