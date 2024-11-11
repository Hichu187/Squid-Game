using UnityEngine;

namespace Game
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Animator _animator;

        public Animator animator { get { if (_animator == null) _animator = GetComponentInChildren<Animator>(); return _animator; } }

        public void SetJumping(bool isJumping)
        {
            if (animator == null)
                return;

            animator.SetBool(HashDictionary.jumping, isJumping);
        }

        public void SetClimbing(bool isClimbing)
        {
            if (animator == null)
                return;

            animator.SetBool(HashDictionary.climbing, isClimbing);
        }

        public void SetClimbY(float y)
        {
            if (animator == null)
                return;

            animator.SetFloat(HashDictionary.climbY, y * 0.5f);
        }

        public void SetVelocityZ(float z)
        {
            if (animator == null)
                return;

            animator.SetFloat(HashDictionary.velocityZ, z);
        }

        public void PlayBlock()
        {
            //animator.Play(HashDictionary.block, 1);
        }

        public void PlayWin()
        {
            animator.Play(HashDictionary.win);
        }

        public void TurnOnBladeBallLayer()
        {
            if (animator == null)
                return;
            animator.SetLayerWeight(1, 1);
        }


        public void SetAnimator(RuntimeAnimatorController ac)
        {
            if (animator == null)
                return;
            animator.runtimeAnimatorController = ac;
        }

    }
}
