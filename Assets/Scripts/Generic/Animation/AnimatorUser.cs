using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using Utilities;

namespace Generic.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorUser : MonoBehaviour
    {
        [SerializeField]protected Animator Animator;

#if UNITY_EDITOR
        [SerializeField]protected AnimatorController AnimatorController;
#endif
        [SerializeField]protected AnimationDict AnimationData;

#if UNITY_EDITOR
        [ContextMenu("Fill Animation Data")]
        protected void FillAnimationData()
        {
            Animator = GetComponent<Animator>();
            AnimationData = new AnimationDict();

            foreach (var clip in AnimatorController.animationClips)
            {
                var eventList = clip.events.Select(clipEvent => new AnimationEventVO() { EventTime = clipEvent.time }).ToList();

                var animationVO = new AnimationVO(clip.name,clip.length,eventList);
                foreach (var animType in AnimType.None.GetEnums())
                {
                    if (!clip.name.Contains(animType.ToString(), StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    AnimationData.Add(animType,animationVO);
                }
            }
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

    public enum AnimType
    {
        None,Walk,Idle
    }

    [Serializable]
    public class AnimationDict : SerializedDictionaryNonEditorModifiable<AnimType, AnimationVO>
    {
        
    }
}