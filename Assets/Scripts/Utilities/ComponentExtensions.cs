using Unity.VisualScripting;
using UnityEngine;

namespace Utilities
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInChildren<T>(this GameObject @this, out T component)
        {
            component = @this.GetComponentInChildren<T>();
            return component != null;
        }

        public static T GetComponentEnsure<T>(this UnityEngine.Object @this) where T : Component
        {
            var component = @this.GetComponent<T>() ?? @this.AddComponent<T>();
            return component;
        }
    }
}