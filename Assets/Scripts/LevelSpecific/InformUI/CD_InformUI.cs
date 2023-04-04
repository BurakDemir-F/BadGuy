using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSpecific.InformUI
{
    [CreateAssetMenu(menuName = "Data/Config/InformUI", fileName = "CD_InformUI", order = 0)]
    public class CD_InformUI : ScriptableObject
    {
        public Color ItemColor;
        public Color TextColor;
        public List<InformUIElement> Elements;
    }

    [System.Serializable]
    public class InformUIElement
    {
        public string Item;
        public string Text;
    }
}