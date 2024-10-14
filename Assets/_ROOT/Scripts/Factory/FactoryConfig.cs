using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class FactoryConfig : ScriptableObjectSingleton<FactoryConfig>
    {
        [Title("Character")]
        [SerializeField] private float _characterBoosterJumpMultiple = 1.3f;

        [Title("Parkour Race")]
        [SerializeField] private float _parkourRaceReviveDelay = 2f;

        [Title("Layer Mask")]
        [SerializeField] private LayerMask _layerMaskLadder;

        public static float characterBoosterJumpMultiple => instance._characterBoosterJumpMultiple;

        public static float parkourRaceReviveDelay => instance._parkourRaceReviveDelay;

        public static LayerMask layerMaskLadder => instance._layerMaskLadder;
    }
}
