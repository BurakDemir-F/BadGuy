using System.Collections.Generic;
using System.IO;
using Generic.SO;
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
    }
}