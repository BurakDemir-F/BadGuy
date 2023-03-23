#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Utilities
{
    public static class AnimatorControllerExtensions
    {
        public static AnimatorControllerParameter GetParameterByName(this AnimatorController controller, string name)
        {
            foreach (var parameter in controller.parameters)
            {
                if (name == parameter.name)
                    return parameter;
            }

            return null;
        }
    }
}

#endif
