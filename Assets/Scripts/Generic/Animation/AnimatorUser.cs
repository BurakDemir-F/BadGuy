using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Generic.Animation
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimatorUser<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] protected Animator Animator;

        [SerializeField] private List<TEnum> _keys;
        [SerializeField] private List<AnimationVO> _values;

#if UNITY_EDITOR
        [SerializeField] protected AnimatorController AnimatorController;
#endif
        protected Dictionary<TEnum, AnimationVO> AnimationData;

        //I can't serialize dictionary in generic use.
        private void Awake()
        {
            AnimationData = new Dictionary<TEnum, AnimationVO>();
            for (var i = 0; i < _keys.Count; i++)
            {
                var @enum = _keys[i];
                AnimationData.Add(@enum, _values[i]);
            }
        }

        public virtual float Animate(TEnum type)
        {
            return 0f;
        }

#if UNITY_EDITOR
        protected void FillAnimationData()
        {
            _keys = new List<TEnum>();
            _values = new List<AnimationVO>();
            Animator = GetComponent<Animator>();
            AnimationData = new Dictionary<TEnum, AnimationVO>();

            foreach (var clip in AnimatorController.animationClips)
            {
                var eventList = clip.events.Select(clipEvent => new AnimationEventVO() { EventTime = clipEvent.time })
                    .ToList();

                var animationVO = new AnimationVO(clip.name, clip.length, eventList);
                foreach (var animType in Enum.GetValues(typeof(TEnum)))
                {
                    if (!clip.name.Contains(animType.ToString(), StringComparison.OrdinalIgnoreCase))
                        continue;

                    AnimationData.Add((TEnum)animType, animationVO);
                }
            }

            _keys = AnimationData.Keys.ToList();
            _values = AnimationData.Values.ToList();
            EditorUtility.SetDirty(this);
        }
#endif
    }

    [Serializable]
    public struct AnimationVO
    {
        public string Name;
        public float Length;
        public List<AnimationEventVO> Events;

        public AnimationVO(string name, float length, List<AnimationEventVO> eventsVo)
        {
            Name = name;
            Length = length;
            Events = eventsVo;
        }
    }

    [Serializable]
    public struct AnimationEventVO
    {
        public float EventTime;
    }
}