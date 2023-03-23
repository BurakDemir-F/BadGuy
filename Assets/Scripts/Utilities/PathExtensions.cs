#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public static class PathExtensions
    {
        public static string GetAssetPath(this Object asset)
        {
            return AssetDatabase.GetAssetPath(asset);
        }
    }
}
#endif
