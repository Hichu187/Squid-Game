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
            SceneManager.LoadScene(1);
        }
    }
}
