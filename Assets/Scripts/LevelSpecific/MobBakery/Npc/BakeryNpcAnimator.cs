using System.Collections.Generic;
using Generic.Animation;
using UnityEngine;

namespace LevelSpecific.MobBakery.Npc
{
    public class BakeryNpcAnimator : AnimatorUser<BakeryNpcAnimType, BakeryNpcAnimation>
    {
#if UNITY_EDITOR
        [ContextMenu("Fill Animation Data")]
        protected void FillAnimations()
        {
            FillAnimationData();
        }
#endif
    }
    
    [System.Serializable]
    public class BakeryNpcAnimation : AnimationComponent<BakeryNpcAnimType>{}

    public enum BakeryNpcAnimType
    {
        None,
        Idle,
        Walk,
        Pick,
        Sit,
        Shoot,
        Jump,
        Death
    }
}