using System;
using System.Collections.Generic;
using Generic.Items;
using Generic.SO;
using UnityEditor;
using UnityEngine;

namespace EditorSpecific
{
    public class BreakableItemCreatorWindow : ItemCreatorWindowBase<CD_ItemConfig>
    {
        private List<Type> _componentsToAdd = new List<Type>()
        {
            typeof(BreakableItem),
            typeof(Rigidbody),
            typeof(BoxCollider)
        };
        
        protected override List<Type> ComponentsToAdd => _componentsToAdd;

        [MenuItem("Tools/Breakable Item Creator")]
        static void Init()
        {
            var window = (BreakableItemCreatorWindow)EditorWindow.GetWindow(typeof(BreakableItemCreatorWindow));
            window.Show();
        }
    }
}