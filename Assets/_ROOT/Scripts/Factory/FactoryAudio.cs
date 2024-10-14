using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryAudio : ScriptableObjectSingleton<FactoryAudio>
    {
        [Title("Sfx")]
        [SerializeField] private AudioConfig _sfxUIButtonClick;

        public static AudioConfig sfxUIButtonClick => instance._sfxUIButtonClick;
    }
}
