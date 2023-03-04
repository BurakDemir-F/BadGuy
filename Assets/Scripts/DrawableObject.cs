using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities;

[System.Serializable]
public class DrawableObject
{

    private bool _isDrawable;

    public bool IsDrawable
    {
        get { return _isDrawable;}
        set
        {
            if (_prefab == null)
            {
                _isDrawable = false;
                Debug.Log("you need to assign prefab");
                return;
            }

            _isDrawable = value;

            if (_isDrawable)
                CreateIndicatorAndParent();
            else
            {
                DestroyObject(indicator);
                if(parent != null && parent.childCount == 0)
                    DestroyObject(parent.gameObject);
            }
            
            if(indicator)
                indicator.gameObject.SetActive(_isDrawable);
        }
    }
    [SerializeField]
    private GameObject _prefab;

    public GameObject Prefab
    {
        get => _prefab;
        set
        {
            if(value == null)
                return;

            if(value == _prefab)
                return;
            
            _prefab = value;
            IsDrawable = true;
        }
    }
    public GameObject indicator { get; private set; }
    public Transform parent{ get; private set; }
    public KeyCode activationKey;
    
    private List<GameObject> _instances;
    public DrawableObject()
    {
        _instances = new();
    }

    public GameObject CreateInstanceAndTrack()
    {
        var newObj = CreateGameObject(_prefab, _prefab.name,!_prefab.IsSceneObject());
        _instances.Add(newObj);
        return newObj;
    }

    public void SetRotationLast()
    {
        _instances[^1].transform.rotation = indicator.transform.rotation;
    }

    public void SetParentLast()
    {
        _instances[^1].transform.SetParent(parent);
    }

    public void DestroyObject(GameObject objToDestroy)
    {
        Object.DestroyImmediate(objToDestroy);
    }
    
    private GameObject CreateGameObject(Object prefab = null, string name = "", bool isPrefabCreation = false)
    {
        if (prefab != null)
        {
            if (isPrefabCreation)
            {
                var prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                prefabInstance.name = GetName();
                return prefabInstance;
            }
            
            var instance = Object.Instantiate(prefab) as GameObject;
            instance.name = GetName();
            return instance;
        }

        var newObj = new GameObject(name == "" ? "Object" : name);
        return newObj;

        string GetName() => name == "" ? prefab.name : name;
    }

    private void CreateIndicatorAndParent()
    {
        if (!indicator)
            indicator = CreateGameObject(_prefab, "Indicator",!_prefab.IsSceneObject());

        if (!parent)
            parent = CreateGameObject(null, $"parent_{_prefab.name}").transform;
    }
    
    private void Clear()
    {
        foreach (var instance in _instances)
        {
            Object.DestroyImmediate(instance);
        }
        
        Object.DestroyImmediate(indicator);
        Object.DestroyImmediate(parent);
    }
}