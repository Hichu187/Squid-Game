using UnityEngine;

namespace Game
{
    public class CharacterKCInputPlayer
    {
        public float moveAxisForward;
        public float moveAxisRight;
        public Quaternion cameraRotation;
        public bool jumpDown;
        public bool jetpackDown;

        public void Reset()
        {
            moveAxisForward = 0f;
            moveAxisRight = 0f;
            cameraRotation = Quaternion.identity;
            jumpDown = false;
            jetpackDown = false;
        }
    }
}