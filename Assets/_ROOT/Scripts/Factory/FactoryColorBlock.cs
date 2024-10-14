using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryColorBlock : ScriptableObjectSingleton<FactoryColorBlock>
    {
        [ListDrawerSettings(AddCopiesLastElement = true, ListElementLabelName = "name")]
        [SerializeField] private ColorBlock_ColorConfig[] _colorConfigs;

        public static ColorBlock_ColorConfig[] colorConfigs => instance._colorConfigs;
    }
}
