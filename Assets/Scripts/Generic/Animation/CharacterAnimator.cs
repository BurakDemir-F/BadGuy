using System;
using Generic.Animation;
using UnityEngine;

namespace Generic.Animation
{
    public class CharacterAnimator : AnimatorUser
    {

        private static readonly int Status = Animator.StringToHash("Status");
        
        //Also you can use AnimType, Action dict.
        public virtual float Animate(AnimType type) 
        {
            switch (type)
            {
                case AnimType.None:
                    Debug.Log("you send wrong anim state, maybe?");
                    return 0f;
                
                case AnimType.Walk:
                    Animator.SetInteger(Status,1);
                    return AnimationData[AnimType.Walk].Length;
                
                case AnimType.Idle:
                    Animator.SetInteger(Status,0);
                    return AnimationData[AnimType.Idle].Length;
                
                default:
                    return 0f;                   
            }
        }

        /*
         * Status
         * 0 - Idle
         * 1 - Walking
         *
         * 
         */
    }
}