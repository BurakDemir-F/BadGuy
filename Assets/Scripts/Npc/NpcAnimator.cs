using System;
using System.Collections.Generic;
using Generic.Animation;
using UnityEngine;

namespace Npc
{
    public class NpcAnimator : AnimatorUser<NpcAnimType>
    {
        [SerializeField] private List<NpcAnimation> _animations;
        private Dictionary<NpcAnimType, NpcAnimation> _animationDict;
        
        public void Initialize()
        {
            if (_animations == null || _animations.Count == 0)
                return;

            _animationDict = new Dictionary<NpcAnimType, NpcAnimation>();
            foreach (var anim in _animations)
            {
                _animationDict.Add(anim.AnimType, anim);
            }
        }
        
        public override float Animate(NpcAnimType type)
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
    }

    public enum NpcAnimType
    {
        None,
        Idle,
        Walk,
        Run,
        Attack
    }
    
    [System.Serializable]
    public class NpcAnimation : AnimationComponent<NpcAnimType>{}
}