using UnityEditor;
using UnityEngine;

namespace Generic.SO
{
    public class ScriptableObjectHelper
    {
        public static TSO Get<TSO>(string name) where TSO : ScriptableObject
        {
            var so = Resources.Load<TSO>(name);
            if (!so)
                so = GetNew<TSO>(name);

            return so;
        }

        public static TSO GetNew<TSO>(string name) where TSO : ScriptableObject
        {
            var newSo = ScriptableObject.CreateInstance<TSO>();
            var path = $"Assets/Resources/{name}.asset";
            AssetDatabase.CreateAsset(newSo,path);
            return newSo;
        }
    }
}