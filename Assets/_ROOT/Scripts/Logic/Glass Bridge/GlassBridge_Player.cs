using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassBridge_Player : MonoBehaviour
    {
        [Title("Reference")]
        public GlassBridge_Gameplay gameplay;

        public bool isComplete = false;

        public void PlayerGoal()
        {
            if (!isComplete)
            {
                isComplete = true;

                gameplay.Result();
            }
        }
    }
}
