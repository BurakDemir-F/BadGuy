using System;

namespace Generic.Animation
{
    [System.Serializable]
    public class AnimationComponent<TEnum> where TEnum : Enum
    {
        public TEnum AnimType;
        public int ConditionCode;
        public string ParameterName;
    }
}