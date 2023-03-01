using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class MeshDrawerWindow : EditorWindow
{
    private Object _objectToDraw;
    private KeyCode _drawKeyCode;
    private bool _overrideEvents;
    private List<GameObject> _drawnObjects;
    private List<GameObject> _allCreatedObjects;

    private bool _isInitialized;
    private bool _isIndicatorCreated;

    private GameObject _installationIndicator;
    private GameObject _parent;

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

        _drawnObjects = new List<GameObject>();
        _allCreatedObjects = new List<GameObject>();
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
        EditorGUILayout.BeginHorizontal();
        _objectToDraw = EditorGUILayout.ObjectField("Object to draw", _objectToDraw, typeof(GameObject), true);
        _drawKeyCode = (KeyCode)EditorGUILayout.EnumPopup("Draw Key Code",_drawKeyCode);
        if (_objectToDraw && !_isIndicatorCreated)
        {
            if (!_parent)
                _parent = CreateGameObject(null, "Parent");

            if (!_allCreatedObjects.Contains(_parent))
                _allCreatedObjects.Add(_parent);

            _installationIndicator = CreateGameObject(_objectToDraw, "Installation Indicator");
            _allCreatedObjects.Add(_installationIndicator);
            _isIndicatorCreated = true;
        }

        if (!_objectToDraw)
        {
            _isIndicatorCreated = false;
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Stop Drawing"))
        {
            if (_installationIndicator)
            {
                DestroyImmediate(_installationIndicator);
                _objectToDraw = null;
            }
        }

        if (GUILayout.Button("Clear all created objects"))
        {
            foreach (var obj in _allCreatedObjects)
            {
                DestroyImmediate(obj);
            }

            _isIndicatorCreated = false;
            _objectToDraw = null;
        }
    }

    private void OnSceneGui(SceneView view)
    {
        if (!_isInitialized || !_isIndicatorCreated)
            return;

        var currentEvent = Event.current;

        var isWorldPosAcquired = GetMouseWorldPos(out var worldPos);

        if (isWorldPosAcquired)
        {
            _installationIndicator.transform.position = worldPos;
        }

        if (currentEvent.type == EventType.KeyDown)
        {
            if (currentEvent.keyCode == _drawKeyCode)
            {
                if (isWorldPosAcquired)
                {
                    var newObj = CreateGameObject(_objectToDraw);
                    newObj.transform.position = worldPos;
                    newObj.transform.rotation = _installationIndicator.transform.rotation;
                    newObj.transform.SetParent(_parent.transform);
                    _drawnObjects.Add(newObj);
                    _allCreatedObjects.Add(newObj);
                }
            }
        }

        if (currentEvent.type == EventType.ScrollWheel)
        {
            var scrollDelta = currentEvent.delta;
            Debug.Log($"scrol delta: {scrollDelta.y}");
            _installationIndicator.transform.Rotate(Vector3.up, 360f * scrollDelta.y * .01f);
        }
        
        currentEvent.type = EventType.Used;
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
}

[System.Serializable]
public class DrawableObject
{
    public GameObject prefab;
    public Transform parent;
    public List<GameObject> instances;
    public KeyCode drawKey;
    public DrawableObject()
    {
        instances = new();
    }
}