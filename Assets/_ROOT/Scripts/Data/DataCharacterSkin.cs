using LFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DataCharacterSkin : LDataBlock<DataCharacterSkin>
    {
        [SerializeField] private Dictionary<string, CharacterSkinData> _datas;

        [SerializeField] private string _current;

        public static string current { get { return instance._current; } set { instance._current = value; } }

        public static CharacterSkinData Get(string key)
        {
            if (!instance._datas.ContainsKey(key))
                instance._datas.Add(key, new CharacterSkinData());

            return instance._datas[key];
        }

        protected override void Init()
        {
            base.Init();

            _datas = _datas ?? new Dictionary<string, CharacterSkinData>();

            // Set first skin as default
            if (string.IsNullOrEmpty(_current))
                _current = FactoryCharacter.skins[0].name;
            
            // Unlock first skin as default
            FactoryCharacter.skins[0].data.Unlock();
        }
    }
}
