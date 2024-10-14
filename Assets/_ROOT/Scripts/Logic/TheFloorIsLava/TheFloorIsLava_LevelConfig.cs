using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TheFloorIsLava_LevelConfig : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] private AssetReferenceGameObject _prefab;
        [SerializeField] private bool _isReward;

        public TheFloorIsLava_LevelData data { get { return DataTheFloorIsLava.GetLevelData(this.name); } }

        public string title { get { return _title; } }
        public Sprite thumbnail { get { return _thumbnail; } }
        public AssetReferenceGameObject prefab { get { return _prefab; } }
        public bool isReward { get { return _isReward; } }
    }
}
