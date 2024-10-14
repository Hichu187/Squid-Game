using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryCharacter : ScriptableObjectSingleton<FactoryCharacter>
    {
        [Title("Skin")]
        [AssetList(Path ="_ROOT/Configs/CharacterSkin")]
        [SerializeField] private CharacterSkinConfig[] _skins;

        public static CharacterSkinConfig[] skins => instance._skins;

        public static CharacterSkinConfig GetSkinCurrent()
        {
            for (int i = 0; i < skins.Length; i++)
            {
                if (skins[i].IsCurrent())
                    return skins[i];
            }

            return skins[0];
        }
    }
}