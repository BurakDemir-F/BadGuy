using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generic.Animation
{
    public class SimpleAnimateSequence<TEnum, TAnimation> : MonoBehaviour
        where TEnum : Enum where TAnimation : AnimationComponent<TEnum>
    {
        [SerializeField] protected List<AnimationSequenceNode<TEnum>> animationSequence;
        [SerializeField] protected bool _isLoop;
        [SerializeField] protected bool _shuffleAnimations;
        private AnimatorUser<TEnum, TAnimation> _animatorUser;
        private Coroutine _sequenceCor;

        protected virtual void Awake()
        {
            _animatorUser = GetComponent<AnimatorUser<TEnum, TAnimation>>();
            if (_shuffleAnimations && animationSequence.Count > 2)
            {
                for (var i = 0; i < animationSequence.Count; i++)
                {
                    var firstRandom = GetRandom();
                    var secondRandom = GetRandom();

                    while (firstRandom == secondRandom)
                    {
                        secondRandom = GetRandom();
                    }

                    var firstItem = animationSequence[firstRandom];
                    var secondItem = animationSequence[secondRandom];
                    animationSequence[firstRandom] = secondItem;
                    animationSequence[secondRandom] = firstItem;
                }
            }

            int GetRandom()
            {
                return Random.Range(0, animationSequence.Count);
            }
        }

        protected void PlaySequence()
        {
            _sequenceCor = StartCoroutine(PlaySequenceCor());
        }

        private void OnDestroy()
        {
            StopSequence();
        }

        private void OnDisable()
        {
            StopSequence();
        }

        protected void StopSequence()
        {
            if (_sequenceCor != null)
                StopCoroutine(_sequenceCor);
        }

        protected IEnumerator PlaySequenceCor()
        {
            if(!_isLoop)
            {
                foreach (var node in animationSequence)
                    yield return new WaitForSeconds(PlayAnim(node));
            }
            else
            {
                var counter = 0;
                while (true)
                {
                    var node = animationSequence[counter % animationSequence.Count];
                    yield return new WaitForSeconds(PlayAnim(node));
                    counter++;
                }
            }

            float PlayAnim(AnimationSequenceNode<TEnum> node)
            {
                var animDuration = _animatorUser.Animate(node.AnimationType);
                if (node.Duration > animDuration)
                    animDuration = node.Duration;
                return animDuration;
            }
        }
    }

    [System.Serializable]
    public class AnimationSequenceNode<TEnum> where TEnum : Enum
    {
        public TEnum AnimationType;
        public float Duration;
        public int Number;
    }
}