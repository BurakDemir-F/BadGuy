using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace EditorSpecific
{
    public class ColliderComponentAdder : EditorWindow
    {
        private Transform _root;

        [MenuItem("Tools/ColliderComponentAdder")]
        static void Init()
        {
            var window = (ColliderComponentAdder)EditorWindow.GetWindow(typeof(ColliderComponentAdder));
            window.Show();
        }

        private void OnGUI()
        {
            _root = Selection.activeTransform;

            if (!_root)
                GUILayout.Label("Select object root from scene or project.");
            else
            {
                GUILayout.Label($"Root Transform Name : {_root.name}");
                ShowAddCollidersButton();
            }
        }

        private void ShowAddCollidersButton()
        {
            if (!GUILayout.Button("Add Colliders Based On NavMesh Obstacle"))
                return;

            var isChangeHappened = false;
            
            foreach (Transform prefabInstance in _root)
            {
                isChangeHappened = false;
                
                if (prefabInstance.gameObject.TryGetComponentInChildren<NavMeshObstacle>(out var obstacle))
                {
                    if(prefabInstance.gameObject.TryGetComponentInChildren<Collider>(out var collider))
                    {
                        collider.transform.position = obstacle.transform.position;
                        collider.transform.rotation = obstacle.transform.rotation;
                        isChangeHappened = true;
                        continue;
                    }

                    var shape = obstacle.shape;
                    var newObj = new GameObject("Capsule Collider");
                    newObj.transform.SetParent(obstacle.transform.parent);
                    switch (shape)
                    {
                        case NavMeshObstacleShape.Capsule:
                            newObj.name = "Capsule Collider";
                            var capsule = newObj.gameObject.AddComponent<CapsuleCollider>();
                            
                            capsule.center = obstacle.center;
                            capsule.height = obstacle.height;
                            capsule.radius = obstacle.radius;
                            break;

                        case NavMeshObstacleShape.Box:
                            newObj.name = "Box Collider";
                            var box = newObj.gameObject.AddComponent<BoxCollider>();
                            
                            box.size = obstacle.size;
                            box.center = obstacle.center;
                            break;
                    }

                    newObj.transform.position = obstacle.transform.position;
                    newObj.transform.rotation = obstacle.transform.rotation;
                    isChangeHappened = true;
                }
                
                if(isChangeHappened)
                {
                    var isPrefabInstance = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefabInstance.gameObject);
                    if(isPrefabInstance)
                        PrefabUtility.ApplyPrefabInstance(prefabInstance.gameObject, InteractionMode.AutomatedAction);
                }

            }
        }
    }
}