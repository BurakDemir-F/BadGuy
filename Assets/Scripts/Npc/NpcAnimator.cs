using System;
using Generic.Animation;
using UnityEngine;

namespace Npc
{
    public class NpcAnimator : AnimatorUser<NpcAnimType>
    {
        private static readonly int Status = Animator.StringToHash("Status");
        
        public override float Animate(NpcAnimType type)
        {
            switch (type)
            {
                case NpcAnimType.Idle:
                    animator.SetInteger(Status,0);
                    return AnimationData[NpcAnimType.Idle].Length;
                case NpcAnimType.Walk:
                    animator.SetInteger(Status,1);
                    return AnimationData[NpcAnimType.Walk].Length;
                case NpcAnimType.Run:
                    animator.SetInteger(Status,2);
                    return AnimationData[NpcAnimType.Run].Length;
                case NpcAnimType.Attack:
                    animator.SetInteger(Status,3);
                    return AnimationData[NpcAnimType.Attack].Length;
            }

            return 0f;
        }
        
#if UNITY_EDITOR
        [ContextMenu("Fill Animation Data")]
        protected void FillAnimations()
        {
            FillAnimationData();
        }
#endif
    }

    public enum NpcAnimType
    {
        None,
        Idle,
        Walk,
        Run,
        Attack
    }
}