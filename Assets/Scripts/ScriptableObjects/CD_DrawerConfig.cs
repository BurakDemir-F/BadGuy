using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DrawerConfig", menuName = "Data/Config/DrawerConfig", order = 0)]
    public class CD_DrawerConfig : ScriptableObject
    {
        public List<DrawableObject> drawableObjects;
        public KeyCode genericDrawKey;
        public KeyCode genericStopKey;

        public static CD_DrawerConfig GetConfig()
        {
            var config = Resources.Load<CD_DrawerConfig>("DrawerConfig");
            if (config == null)
            {
                config = CreateInstance();
            }

            return config;
        }
        
        private static CD_DrawerConfig CreateInstance()
        {
            var configSo = ScriptableObject.CreateInstance<CD_DrawerConfig>();
            AssetDatabase.CreateAsset(configSo,"Assets/Resources/DrawerConfig.asset");
            return configSo;
        }
    }
}