using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace EditorSpecific
{
    [CreateAssetMenu(menuName = "Data/Config/MeshDrawer Safe Config", fileName = "CD_DrawerSaveConfig", order = 0)]
    public class CD_DrawerSaveConfig : ScriptableObject
    {
        public List<SaveConfig> SaveConfigs;
    }

    [Serializable]
    public class SaveConfig
    {
        public string SaveName;
        public CD_DrawerConfig Config;
    }
}