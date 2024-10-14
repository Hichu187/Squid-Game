using UnityEngine;

namespace Game
{
    public class MiniGameConfig : ScriptableObject
    {
        [SerializeField] private MiniGameType _type;
        [SerializeField] private string _title;
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] bool _isMostPlayed;
        [SerializeField] private string _sceneName;
        [SerializeField] bool _isComingSoon;

        public MiniGameType type { get { return _type; } }
        public string title { get { return _title; } }
        public Sprite thumbnail { get { return _thumbnail; } }
        public bool isMostPlayed { get { return _isMostPlayed; } }
        public string sceneName { get { return _sceneName; } }
        public bool isComingSoon { get { return _isComingSoon; } }
    }
}
