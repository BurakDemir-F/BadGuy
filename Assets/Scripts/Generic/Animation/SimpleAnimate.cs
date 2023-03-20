using System;
using UnityEngine;

namespace Generic.Animation
{
    public class SimpleAnimate<TEnum,TAnimation> : MonoBehaviour, IAnimatable where TEnum : Enum where TAnimation : AnimationComponent<TEnum>
    {
        [SerializeField] private TEnum _animationType;
        private AnimatorUser<TEnum, TAnimation> _animatorUser;
        protected virtual void Awake()
        {
            _animatorUser = GetComponent<AnimatorUser<TEnum, TAnimation>>();
        }

        public virtual float Animate(TEnum type)
        {
            return _animatorUser.Animate(type);
        }
        
        public virtual float Animate()
        {
            _animatorUser ??= GetComponent<AnimatorUser<TEnum, TAnimation>>();
            return _animatorUser.Animate(_animationType);
        }
    }
}