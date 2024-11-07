using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MainManager : MonoBehaviour
    {
        public void PlayChallenge()
        {
            SceneLoaderHelper.Load(1);
            DataMainGame.levelIndex = 0;
            DataMainGame.isChallenge = true;
        }

        public void PlayMiniGame(int scene)
        {
            DataMainGame.isChallenge = false;
            SceneLoaderHelper.Load(scene + 2);
        }
    }
}
