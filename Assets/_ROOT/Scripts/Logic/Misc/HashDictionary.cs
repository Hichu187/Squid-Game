using UnityEngine;

namespace Game
{
    public static class HashDictionary
    {
        public static int velocityX = Animator.StringToHash("VelocityX");
        public static int velocityZ = Animator.StringToHash("VelocityZ");

        public static int climbY = Animator.StringToHash("ClimbY");

        public static int jumping = Animator.StringToHash("Jumping");
        public static int climbing = Animator.StringToHash("Climbing");

        public static int win = Animator.StringToHash("Win");

        public static int block = Animator.StringToHash("SlashEnd");
    }
}
