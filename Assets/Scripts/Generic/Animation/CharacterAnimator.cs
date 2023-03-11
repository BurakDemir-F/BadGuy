using System.Collections.Generic;
using UnityEngine;

namespace Generic.Animation
{
    public class CharacterAnimator : AnimatorUser<AnimType>
    {
        [SerializeField] private List<CharacterAnimation> _animations;
        private Dictionary<AnimType, CharacterAnimation> _animationDict;
        
        public void Initialize()
        {
            if (_animations == null || _animations.Count == 0)
                return;

            _animationDict = new Dictionary<AnimType, CharacterAnimation>();
            foreach (var anim in _animations)
            {
                _animationDict.Add(anim.AnimType, anim);
            }
        }

        public override float Animate(AnimType type)
        {
            var anim = _animationDict[type];
            animator.SetInteger(anim.ParameterName,anim.ConditionCode);
            return AnimationData[type].Length;
        }
        
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
        None,Walk,Idle,Pull
    }
    
    [System.Serializable]
    public class CharacterAnimation : AnimationComponent<AnimType>{}
}