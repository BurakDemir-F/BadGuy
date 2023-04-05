using System.Collections.Generic;
using Generic.Animation;
using UnityEngine;

namespace Npc
{
    public class NpcAnimator : AnimatorUser<NpcAnimType, NpcAnimation>
    {

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
        Attack,
        Hi,
        Pull,
        Win,
        Stabbing
    }
    
    [System.Serializable]
    public class NpcAnimation : AnimationComponent<NpcAnimType>{}
}