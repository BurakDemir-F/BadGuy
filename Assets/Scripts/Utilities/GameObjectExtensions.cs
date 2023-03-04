using UnityEngine;

namespace Utilities
{
    public static class GameObjectExtensions
    {
        public static bool IsSceneObject(this GameObject go)
        {
            return go.scene.name != null;
        }
    }
}