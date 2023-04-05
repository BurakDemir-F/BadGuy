using System.Collections.Generic;
using UnityEngine;

namespace Generic.Animation
{
    public class CharacterAnimator : AnimatorUser<AnimType,CharacterAnimation>
    {

#if UNITY_EDITOR
        [ContextMenu("Fill Animation Data")]
        protected void FillAnimations()
        {
            FillAnimationData();
        }
#endif
        
        /*
         * Status
         * 0 - Idle
         * 1 - Walking
         * 
         * 5- Pull
         */
    }
    
    public enum AnimType
    {
        None,Walk,Idle,Pull,Catched
    }
    
    [System.Serializable]
    public class CharacterAnimation : AnimationComponent<AnimType>{}
}