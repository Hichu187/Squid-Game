using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class BladeBall_LevelConfig : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] private AssetReference _prefabAsset;
        [SerializeField] private bool _isReward;
        [SerializeField] List<AIType> _listAI;

        public string title { get { return _title; } }
        public Sprite thumbnail { get { return _thumbnail; } }
        public AssetReference prefabAsset { get { return _prefabAsset; } }
        public bool isReward { get { return _isReward; } }

        public List<AIType> listAi { get { return _listAI; } }
    }
}
