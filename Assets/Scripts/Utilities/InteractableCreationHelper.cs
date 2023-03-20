using Generic.Items;
using Generic.Items.SO;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class InteractableCreationHelper : MonoBehaviour
    {
        public CD_ThrowableItem throwableItemData;
        public int publicField;
        public int PublicProperty { get; set; }

        private int _privateField;
        private int PrivateProperty { get; set; }

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(InteractableCreationHelper))]
    public class CreationHelperInspector : Editor
    {
        private InteractableCreationHelper _target;

        private void OnEnable()
        {
            _target = target as InteractableCreationHelper;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            CreateInteractablesButton();
            LogPropertiesButton();
        }

        private void CreateInteractablesButton()
        {
            if (!GUILayout.Button("Create Interactables"))
                return;

            var childCount = _target.transform.childCount;
            var holdItemCount = 0;
            var breakItemCount = 0;
            var holdBreakRatio = .3f;

            foreach (Transform child in _target.transform)
            {
                var breakItem = child.GetComponent<BreakableItem>();
                var holdItem = child.GetComponent<MovementBasedRotationItem>();

                var isBreakItem = breakItem != null;
                var isHoldItem = holdItem != null;

                var isNeedCreation = !isBreakItem && !isHoldItem;

                if (!isNeedCreation)
                {
                    if (isBreakItem)
                        breakItemCount++;
                    else if (isHoldItem)
                        holdItemCount++;

                    continue;
                }

                if ((float)holdItemCount / breakItemCount > holdBreakRatio)
                {
                    child.gameObject.AddComponent<BreakableItem>();
                    breakItemCount++;
                }
                else
                {
                    child.gameObject.AddComponent<MovementBasedRotationItem>();
                    holdItemCount++;
                }

                child.gameObject.GetComponent<MeshCollider>().convex = true;
                var boxCollider = child.gameObject.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                boxCollider.size *= 1.3f;
                child.gameObject.AddComponent<Rigidbody>();
            }
        }

        private void LogPropertiesButton()
        {
            if(!GUILayout.Button("Log All Properties"))
                return;
            
            var prop = serializedObject.GetIterator();
            var enumarator = prop.GetEnumerator();

            // foreach (SerializedProperty p in prop)
            // {
            //     Debug.Log($"p name : {p.name}");
            // }
            
            while (enumarator.MoveNext())
            {
                prop = enumarator.Current as SerializedProperty;
                Debug.Log($"prop name: {prop.name}");
            }
        }

#endif
    }
}