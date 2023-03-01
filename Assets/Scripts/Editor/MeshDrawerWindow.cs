using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class MeshDrawerWindow : EditorWindow
{
    private List<DrawableObject> _drawableObjects;
    private DrawableObject _currentDrawable;
    private bool _isInitialized;
    private KeyCode _genericDrawKey;
    private KeyCode _genericStopKey;

    [MenuItem("Tools/Mesh Drawer")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = (MeshDrawerWindow)EditorWindow.GetWindow(typeof(MeshDrawerWindow));
        window.Show();
    }

    private void Initialize()
    {
        if (_isInitialized)
            return;

        _drawableObjects = new List<DrawableObject>();
        _drawableObjects.Add(new DrawableObject());
        _currentDrawable = _drawableObjects[0];
        _isInitialized = true;
    }

    private void OnEnable()
    {
        Initialize();
        SceneView.duringSceneGui += OnSceneGui;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGui;
    }

    private void OnGUI()
    {
        DrawGenericKeyCode();
        DrawDrawables();
        ShowAddDrawableButton();
    }

    private void DrawGenericKeyCode()
    {
        _genericDrawKey = (KeyCode)EditorGUILayout.EnumPopup("Generic Draw Key",_genericDrawKey);
        _genericStopKey = (KeyCode)EditorGUILayout.EnumPopup("Generic Stop Key",_genericStopKey);
    }

    private void DrawDrawables()
    {
        foreach (var drawableObject in _drawableObjects)
        {
            EditorGUILayout.BeginHorizontal();

            drawableObject.Prefab = (GameObject)EditorGUILayout.ObjectField(drawableObject.Prefab == null
                    ? "Prefab"
                    : drawableObject.Prefab.name,
                drawableObject.Prefab,
                typeof(GameObject),
                true);

            drawableObject.activationKey = (KeyCode)EditorGUILayout.EnumPopup("Activation Key",drawableObject.activationKey);

            EditorGUILayout.EndHorizontal();
        }
    }

    private void ShowAddDrawableButton()
    {
        if (GUILayout.Button("Add New Drawable Object"))
        {
            _drawableObjects.Add(new DrawableObject());
        }
    }

    private void OnSceneGui(SceneView view)
    {
        CheckDrawableActivationStatus();
        
        if (!_isInitialized || !_currentDrawable.IsDrawable)
            return;

        var currentEvent = Event.current;

        var isWorldPosAcquired = GetMouseWorldPos(out var worldPos);

        if (isWorldPosAcquired)
            _currentDrawable.indicator.transform.position = worldPos;

        var isDrawable = currentEvent.type == EventType.KeyDown && currentEvent.keyCode == _genericDrawKey &&
                         isWorldPosAcquired;

        if (isDrawable)
        {
            var newObj = _currentDrawable.CreateInstanceAndTrack();
            newObj.transform.position = worldPos;
            _currentDrawable.SetRotationLast();
            _currentDrawable.SetParentLast();
        }

        if (currentEvent.type == EventType.ScrollWheel)
        {
            var scrollDelta = currentEvent.delta;
            _currentDrawable.indicator.transform.Rotate(Vector3.up, 360f * scrollDelta.y * .01f);
        }
        
        currentEvent.type = EventType.Used;
    }

    private void CheckDrawableActivationStatus()
    {
        var currentEvent = Event.current;
        if(currentEvent.type != EventType.KeyDown)
            return;

        var keyCode = currentEvent.keyCode;

        if (keyCode == _genericStopKey)
        {
            foreach (var drawable in _drawableObjects)
            {
                drawable.IsDrawable = false;
            }
            return;
        }
        
        DrawableObject drawableObject = null;
        
        foreach (var drawable in _drawableObjects)
        {
            if (drawable.activationKey != keyCode)
                continue;

            drawableObject = drawable;
        }
        
        if(drawableObject == null)
            return;

        drawableObject.IsDrawable = true;
        _currentDrawable = drawableObject;

        foreach (var drawable in _drawableObjects)
        {
            if (drawable != drawableObject)
                drawable.IsDrawable = false;
        }
    }

    private bool GetMouseWorldPos(out Vector3 position)
    {
        var mousePos = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePos);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            position = hitInfo.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}

[System.Serializable]
public class DrawableObject
{

    private bool _isDrawable;

    public bool IsDrawable
    {
        get { return _isDrawable;}
        set
        {
            _isDrawable = value;
            if(indicator)
                indicator.gameObject.SetActive(_isDrawable);
        }
    }
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

            if (!indicator)
            {
                indicator = CreateGameObject(_prefab, "Indicator");
            }

            if (!parent)
            {
                parent = CreateGameObject(null, "parent").transform;
            }

            IsDrawable = true;
        }
    }
    public GameObject indicator { get; private set; }
    public Transform parent{ get; private set; }
    public List<GameObject> instances;
    public KeyCode activationKey;
    public DrawableObject()
    {
        instances = new();
    }

    public GameObject CreateInstanceAndTrack()
    {
        var newObj = CreateGameObject(Prefab, indicator.name);
        instances.Add(newObj);
        return newObj;
    }

    public void SetRotationLast()
    {
        instances[^1].transform.rotation = indicator.transform.rotation;
    }

    public void SetParentLast()
    {
        instances[^1].transform.SetParent(parent);
    }
    
    private GameObject CreateGameObject(Object prefab = null, string name = "")
    {
        if (prefab != null)
        {
            var prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            prefabInstance.name = name == "" ? prefabInstance.name : name;
            return prefabInstance;
        }

        var newObj = new GameObject(name == "" ? "Object" : name);
        return newObj;
    }

    private void Clear()
    {
        foreach (var instance in instances)
        {
            Object.DestroyImmediate(instance);
        }
        
        Object.DestroyImmediate(indicator);
        Object.DestroyImmediate(parent);
    }
}