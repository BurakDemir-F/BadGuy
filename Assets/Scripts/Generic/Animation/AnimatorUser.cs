using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEditor;
#endif


namespace Generic.Animation
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimatorUser<TEnum, TAnimation> : MonoBehaviour 
        where TEnum : Enum
        where TAnimation : AnimationComponent<TEnum>
    {
        [SerializeField] protected Animator animator;
        [SerializeField] private List<TEnum> _keys;
        [SerializeField] private List<AnimationVO> _values;

#if UNITY_EDITOR
        [SerializeField] protected AnimatorController animatorController;
#endif
        protected Dictionary<TEnum, AnimationVO> AnimationData;

        [SerializeField] private List<TAnimation> _animations;
        private Dictionary<TEnum, TAnimation> _animationDict;

        public void SetController(AnimatorController controller)
        {
            animatorController = controller;
        }

        public void SetAnimator(Animator anim)
        {
            animator = anim;
        }
        
        private void Awake()
        {
            AnimationData = new Dictionary<TEnum, AnimationVO>();
            for (var i = 0; i < _keys.Count; i++)
            {
                var @enum = _keys[i];
                AnimationData.Add(@enum, _values[i]);
            }
            
            Initialize();
        }
        
        private void Initialize()
        {
            if (_animations == null || _animations.Count == 0)
                return;

            _animationDict = GetTypeAnimationDictionary();
            foreach (var anim in _animations)
            {
                _animationDict.Add(anim.AnimType, anim);
            }
        }

        protected abstract Dictionary<TEnum, TAnimation> GetTypeAnimationDictionary();

        public virtual float Animate(TEnum type)
        {
            var anim = _animationDict[type];
            anim.PlayAnimationOnAnimator(animator);
            return AnimationData[type].Length;
        }

        #region Editor
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

            // foreach (var state in controller.layers[0].stateMachine.states)
            // {
            //     
            //     Debug.Log($"state name: {state.state.name}, transition count:{state.state.transitions.Length}");
            //     
            //     foreach (var transition in state.state.transitions)
            //     {
            //         var sourceState = state.state;
            //         var destinationState = transition.destinationState;
            //         
            //         foreach (var condition in transition.conditions)
            //         {
            //             foreach (var animKeyPair in AnimationData)
            //             {
            //                 var type = animKeyPair.Key;
            //                 var anim = animKeyPair.Value;
            //                 
            //                 var parameter = animatorController.GetParameterByName(condition.parameter);
            //                 if (state.state.motion.name == anim.Name)
            //                 {
            //                     var parameterVo = new AnimationParameterVO();
            //                     parameterVo.ParameterName = parameter.name;
            //                     parameterVo.ParameterType = parameter.type;
            //                     SolveCondition(condition,ref parameterVo);
            //                     anim.Parameters = parameterVo;
            //                 }
            //             }
            //         }
            //     }
            // }

            _keys = AnimationData.Keys.ToList();
            _values = AnimationData.Values.ToList();
            EditorUtility.SetDirty(this);
        }

        // private void SolveCondition(AnimatorCondition condition, ref AnimationParameterVO vo)
        // {
        //     var mode = condition.mode;
        //     
        //     switch (mode)
        //     {
        //         case AnimatorConditionMode.If:
        //             vo.BoolParameter = true;
        //             break;
        //         case AnimatorConditionMode.IfNot:
        //             vo.BoolParameter = false;
        //             break;
        //         case AnimatorConditionMode.Greater:
        //             Debug.Log("not supported now!");
        //             break;
        //         case AnimatorConditionMode.Less:
        //             Debug.Log("not supported now!");
        //             break;
        //         case AnimatorConditionMode.Equals:
        //             vo.FloatParameter = condition.threshold;
        //             break;
        //         case AnimatorConditionMode.NotEqual:
        //             Debug.Log("not supported now!");
        //             break;
        //     }
        // }
#endif
        #endregion

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
    
    [System.Serializable]
    public class AnimationComponent<TEnum> where TEnum : Enum
    {
        public TEnum AnimType;
        public ParameterType ParameterType;
        public int ConditionCode;
        public string ParameterName;

        public void PlayAnimationOnAnimator(Animator animator)
        {
            switch (ParameterType)
            {
                case ParameterType.Int:
                    animator.SetInteger(ParameterName,ConditionCode);
                    break;
                
                case ParameterType.Trigger:
                    animator.SetTrigger(ParameterName);
                    break;
            }
        }
    }

    public enum ParameterType
    {
        None,
        Int,
        Trigger
    }
}