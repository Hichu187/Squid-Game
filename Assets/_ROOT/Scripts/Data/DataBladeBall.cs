using LFramework;
using UnityEngine;

namespace Game
{
    public class DataBladeBall : LDataBlock<DataBladeBall>
    {
        [SerializeField] private int _levelIndex;

        [SerializeField] private int _loseCount;

        [SerializeField] private bool _tutorialCompleted;

        public static int levelIndex { get { return instance._levelIndex; } set { instance._levelIndex = value; } }

        public static int loseCount { get { return instance._loseCount; } set { instance._loseCount = value; } }

        public static bool tutorialCompleted { get { return instance._tutorialCompleted; } set { instance._tutorialCompleted = value; } }
    }
}
