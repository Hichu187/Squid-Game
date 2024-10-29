using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DataMainGame : LDataBlock<DataMainGame>
    {
        [SerializeField] private int _levelIndex;

        [SerializeField] private bool _isChallenge;

        public static int levelIndex { get { return instance._levelIndex; } set { instance._levelIndex = value; } }

        public static bool isChallenge { get { return instance._isChallenge; } set { instance._isChallenge = value; } }
    }
}
