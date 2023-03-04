using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

public class MeshDrawer : EditorWindow
{
    private List<DrawableObject> _drawableObjects;
    private DrawableObject _currentDrawable;
    private bool _isInitialized;
    private KeyCode _genericDrawKey = KeyCode.D;
    private KeyCode _genericStopKey = KeyCode.R;
    private LayerMask _drawableLayerMask;

    [MenuItem("Tools/Mesh Drawer")]
    static void Init()
    {
        var window = (MeshDrawer)EditorWindow.GetWindow(typeof(MeshDrawer));
        window.Show();
    }

    private void Initialize()
    {
        if (_isInitialized)
            return;

        _drawableObjects = new List<DrawableObject>();
        _drawableObjects.Add(new DrawableObject());
        _isInitialized = true;
    }

    private void OnEnable()
    {
        Initialize();
        Load();
        SceneView.duringSceneGui += OnSceneGui;
    }

    private void OnDisable()
    {
        Save();
        SceneView.duringSceneGui -= OnSceneGui;
    }

    private void OnGUI()
    {
        DrawEnums();
        DrawDrawables();
        ShowAddDrawableButton();
    }

    private void DrawEnums()
    {
        _genericDrawKey = (KeyCode)EditorGUILayout.EnumPopup("Generic Draw Key",_genericDrawKey);
        _genericStopKey = (KeyCode)EditorGUILayout.EnumPopup("Generic Stop Key",_genericStopKey);
        _drawableLayerMask = EditorGUILayout.LayerField("Drawable Layer", _drawableLayerMask);
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
        
        if(_currentDrawable == null)
            return;
        
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
        var layerMask = 1 << _drawableLayerMask;
        if (Physics.Raycast(ray,out var hitInfo,Mathf.Infinity,layerMask))
        {
            position = hitInfo.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    private void Save()
    {
        var config = CD_DrawerConfig.GetConfig();
        config.drawableObjects = _drawableObjects;
        config.genericDrawKey = _genericDrawKey;
        config.genericStopKey = _genericStopKey;
    }

    private void Load()
    {
        var config = CD_DrawerConfig.GetConfig();
        _drawableObjects = config.drawableObjects;
        _genericDrawKey = config.genericDrawKey;
        _genericStopKey = config.genericStopKey;

    }
}