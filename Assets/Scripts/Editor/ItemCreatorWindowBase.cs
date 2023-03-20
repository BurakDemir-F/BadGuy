using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generic.Items;
using Generic.SO;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace EditorSpecific
{
    public abstract class ItemCreatorWindowBase<TSO> : EditorWindow where TSO : ScriptableObject,IItemCreationVOProvider
    {
        private List<ItemCreationVO> CreationItems;
        private bool _isInitialized;
        private string _savePath;
        private bool CanCreateItems => PrefabModel != null && RootObjectNameForPlacingMesh != null;

        private GameObject PrefabModel;
        private string RootObjectNameForPlacingMesh;
        private bool _getReferencesFromFolder;
        protected virtual List<Type> ComponentsToAdd { get; }
        
        private void OnEnable()
        {
            Initialize();
            Load();
        }

        private void OnDisable()
        {
            Save();
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;

            CreationItems = new List<ItemCreationVO>();
        }

        private void OnGUI()
        {
            ShowCreationItems();
            AddNewCreationItemButton();
            ShowCreateItemsButton();
            ShowRemoveItemButton();
        }

        private void ShowCreationItems()
        {
            EditorGUILayout.BeginVertical();
            PrefabModel = (GameObject)EditorGUILayout.ObjectField("Prefab Model",
                PrefabModel,
                typeof(GameObject),
                true);
            RootObjectNameForPlacingMesh = EditorGUILayout.TextField("Root Object Name For Placing Mesh",
                RootObjectNameForPlacingMesh);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            
            foreach (var item in CreationItems)
            {
                EditorGUILayout.BeginVertical();
                item.NewPrefabName = EditorGUILayout.TextField("New Prefab Name", item.NewPrefabName);
                item.MeshPrefab = (GameObject)EditorGUILayout.ObjectField("Mesh Model",
                    item.MeshPrefab,
                    typeof(GameObject),
                    true);
                EditorGUILayout.EndVertical();
            }
        }

        private void AddNewCreationItemButton()
        {
            if (GUILayout.Button("Add New Item"))
            {
                CreationItems.Add(new ItemCreationVO());
            }
        }

        private void ShowRemoveItemButton()
        {
            if(!GUILayout.Button("Remove item"))
                return;

            var count = CreationItems.Count;
            
            if(count == 0)
                return;
            CreationItems.RemoveAt(count - 1);
        }

        private void ShowCreateItemsButton()
        {
            if (!GUILayout.Button("Create Items"))
                return;

            if (!CanCreateItems)
            {
                Debug.Log("!Empty data");
                return;
            }
            
            _savePath = EditorUtility.OpenFolderPanel("Prefab save location", _savePath, "");

            foreach (var item in CreationItems)
            {
                var newPrefabRootInstance = PrefabUtility.InstantiatePrefab(PrefabModel) as GameObject;
                if (!newPrefabRootInstance)
                    continue;
                
                PrefabUtility.UnpackPrefabInstance(newPrefabRootInstance,
                    PrefabUnpackMode.Completely,
                    InteractionMode.AutomatedAction);
                
                ClearComponents(newPrefabRootInstance);

                foreach (var componentType in ComponentsToAdd)
                {
                    newPrefabRootInstance.AddComponent(componentType);
                }

                var meshRoot = newPrefabRootInstance.transform.Find(RootObjectNameForPlacingMesh);
                var newMeshPrefabInstance = PrefabUtility.InstantiatePrefab(item.MeshPrefab) as GameObject;
                newMeshPrefabInstance.transform.SetParent(meshRoot);
                newPrefabRootInstance.name = item.NewPrefabName;

                var localPath = _savePath.GetLocalPathFromAbsolute();
                localPath = Path.Combine(localPath, $"{item.NewPrefabName}.prefab");
                CreatePrefabAndSave(newPrefabRootInstance, localPath);
                DestroyImmediate(newPrefabRootInstance);
            }

            void ClearChildren(Transform t)
            {
                while (t.childCount > 0)
                {
                    DestroyImmediate(t.GetChild(0).gameObject);
                }
            }

            void ClearComponents(GameObject obj)
            {
                var baseItem = obj.GetComponent<BaseItem>();
                if(baseItem)
                    DestroyImmediate(baseItem);
                
                var allComponents = obj.GetComponents<Component>();

                for (int i = 0; i < allComponents.Length; i++)
                {
                    if(allComponents[i] is Transform)
                        continue;
                    DestroyImmediate(allComponents[i]);
                }
            }
        }
        protected virtual void CreatePrefabAndSave(GameObject go, string localPath)
        {
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(go, localPath, InteractionMode.AutomatedAction,
                out var prefabSuccess);

            var logString = prefabSuccess
                ? $"Prefab was saved successfully: {go.name}"
                : "Prefab failed to save: {go.name}";

            Debug.Log(logString);
        }

        private void Load()
        {
            var so = ScriptableObjectHelper.Get<TSO>(typeof(TSO).GetTypeName());
            CreationItems = so.Items;
        }

        private void Save()
        {
            var so = ScriptableObjectHelper.Get<TSO>(typeof(TSO).GetTypeName());
            so.Items = CreationItems;
        }
    }
}