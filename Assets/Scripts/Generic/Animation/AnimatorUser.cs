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
        [FormerlySerializedAs("Animator")] [SerializeField]
        protected Animator animator;

        [SerializeField] private List<TEnum> _keys;
        [SerializeField] private List<AnimationVO> _values;

#if UNITY_EDITOR
        [FormerlySerializedAs("AnimatorController")] [SerializeField]
        protected AnimatorController animatorController;
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
            animator = GetComponent<Animator>();
            AnimationData = new Dictionary<TEnum, AnimationVO>();

            var controller = animatorController;

            foreach (var clip in controller.animationClips)
            {
                var eventList = clip.events.Select(clipEvent => new AnimationEventVO() { EventTime = clipEvent.time })
                    .ToList();

                var animationVO = new AnimationVO(clip.name, clip.length, eventList,new AnimationParameterVO());
                foreach (var animType in Enum.GetValues(typeof(TEnum)))
                {
                    if (!clip.name.Contains(animType.ToString(), StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (AnimationData.ContainsKey((TEnum)animType)) continue;

                    AnimationData.Add((TEnum)animType, animationVO);
                }
            }

            foreach (var state in controller.layers[0].stateMachine.states)
            {
                
                Debug.Log($"state name: {state.state.name}, transition count:{state.state.transitions.Length}");
                
                foreach (var transition in state.state.transitions)
                {
                    var sourceState = state.state;
                    var destinationState = transition.destinationState;
                    
                    foreach (var condition in transition.conditions)
                    {
                        foreach (var animKeyPair in AnimationData)
                        {
                            var type = animKeyPair.Key;
                            var anim = animKeyPair.Value;
                            
                            var parameter = animatorController.GetParameterByName(condition.parameter);
                            if (state.state.motion.name == anim.Name)
                            {
                                var parameterVo = new AnimationParameterVO();
                                parameterVo.ParameterName = parameter.name;
                                parameterVo.ParameterType = parameter.type;
                                SolveCondition(condition,ref parameterVo);
                                anim.Parameters = parameterVo;
                            }
                        }
                    }
                }
            }

            _keys = AnimationData.Keys.ToList();
            _values = AnimationData.Values.ToList();
            EditorUtility.SetDirty(this);
        }

        private void SolveCondition(AnimatorCondition condition, ref AnimationParameterVO vo)
        {
            var mode = condition.mode;
            
            switch (mode)
            {
                case AnimatorConditionMode.If:
                    vo.BoolParameter = true;
                    break;
                case AnimatorConditionMode.IfNot:
                    vo.BoolParameter = false;
                    break;
                case AnimatorConditionMode.Greater:
                    Debug.Log("not supported now!");
                    break;
                case AnimatorConditionMode.Less:
                    Debug.Log("not supported now!");
                    break;
                case AnimatorConditionMode.Equals:
                    vo.FloatParameter = condition.threshold;
                    break;
                case AnimatorConditionMode.NotEqual:
                    Debug.Log("not supported now!");
                    break;
            }
        }
#endif
    }

    [Serializable]
    public class AnimationVO
    {
        public string Name;
        public float Length;
        public List<AnimationEventVO> Events;
        public AnimationParameterVO Parameters;

        public AnimationVO(string name, float length, List<AnimationEventVO> eventsVo, AnimationParameterVO parameters)
        {
            Name = name;
            Length = length;
            Events = eventsVo;
            Parameters = parameters;
        }
    }

    [Serializable]
    public class AnimationParameterVO
    {
        public string ParameterName;
        public AnimatorControllerParameterType ParameterType;
        public bool BoolParameter;
        public int IntParameter;
        public float FloatParameter;
        public bool TriggerParameter;
    }

    [Serializable]
    public class AnimationEventVO
    {
        public float EventTime;
    }

    public enum AnimationParameterType
    {
        None,
        Float,
        Int,
        Bool,
        Trigger
    }
}