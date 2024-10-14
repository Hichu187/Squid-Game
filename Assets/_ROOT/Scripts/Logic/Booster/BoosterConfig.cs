using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class BoosterConfig : ScriptableObject
    {
        [SerializeField] private BoosterType _type;
        [SerializeField] private AssetReferenceGameObject _model;
        [SerializeField] private float _duration;

        public BoosterType type { get { return _type; } }
        public AssetReferenceGameObject model { get { return _model; } }
        public float duration { get { return _duration; } }
    }
}
