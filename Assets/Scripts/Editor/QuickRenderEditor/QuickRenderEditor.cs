using RenderHelper;
using UnityEditor;
using UnityEngine;
using Utilities;
using Task = System.Threading.Tasks.Task;

namespace EditorSpecific.QuickRenderEditor
{
    public class QuickRenderEditor : EditorWindow
    {
        private string _savePath;
        private string _objectPath;
        private RenderTexture _texture;
        private RenderTexture _cameraRenderTexture;
        private Camera _camera;
        private Vector2Int _renderResolution = new Vector2Int(256, 256);
        private int _animFrameCount = 1;
        private int _nameCounter;

        [MenuItem("Tools/QuickRenderEditor")]
        public static void Init()
        {
            var window = GetWindow<QuickRenderEditor>();
            window.Show();
        }

        private void OnEnable()
        {
            CreateRenderTextures();
        }

        private void CreateRenderTextures()
        {
            _texture = new RenderTexture(_renderResolution.x, _renderResolution.y, 0, RenderTextureFormat.Default,
                RenderTextureReadWrite.Default);
            _cameraRenderTexture = new RenderTexture(_renderResolution.x, _renderResolution.y, 0,
                RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        }

        private void OnDisable()
        {
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            ShowPath();
            ShowAnimFrameCount();
            ShowRenderResolution();
            ShowCameraLayout();
            ShowRenderButton();
            ShowRenderAsAnimation();
            GUILayout.EndVertical();
        }

        private void ShowRenderResolution()
        {
            var resolution = _renderResolution;
            _renderResolution = EditorGUILayout.Vector2IntField("Resolution", _renderResolution);
            if (_renderResolution != resolution)
            {
                CreateRenderTextures();
            }
        }

        private void ShowCameraLayout()
        {
            _camera = EditorGUILayout.ObjectField("Render Camera", _camera, typeof(Camera), true) as Camera;
        }

        private void ShowPath()
        {
            GUILayout.BeginHorizontal();
            // GUILayout.Label(_savePath);
            EditorGUILayout.LabelField(_savePath);
            if (GUILayout.Button("Choose Save Path"))
            {
                _savePath = EditorUtility.OpenFolderPanel("Texture save location", _savePath, "");
                //_savePath = _savePath + "/" + "texture.png";
            }

            GUILayout.EndHorizontal();
        }

        private void ShowRenderAsAnimation()
        {
            if (!GUILayout.Button("Render As Animation"))
                return;

            if (string.IsNullOrEmpty(_savePath))
            {
                Debug.Log("Save path is empty");
                return;
            }

            if (_animFrameCount == 0)
            {
                Debug.Log("Anim Frame Count can not be zero");
                return;
            }

            RenderAsAnimation();
        }

        private async void RenderAsAnimation()
        {
            var newObjName = TryGetObjectName(out var targetName) ? targetName : "texture";
            _objectPath = $"{_savePath}/{newObjName}{_nameCounter++}.png";

            for (int i = 0; i < _animFrameCount; i++)
            {
                await RenderAsync();
                _objectPath = $"{_savePath}/{newObjName}{_nameCounter++}.png";
            }
        }

        private void ShowAnimFrameCount()
        {
            _animFrameCount = EditorGUILayout.IntField("Animation Frame Count", _animFrameCount);
        }

        private void ShowRenderButton()
        {
            if (!GUILayout.Button("Render"))
                return;

            if (string.IsNullOrEmpty(_savePath))
            {
                Debug.Log("Save path is empty");
                return;
            }

            var newObjName = TryGetObjectName(out var targetName) ? targetName : "texture";
            _objectPath = $"{_savePath}/{newObjName}.png";

            RenderAsync();
        }

        private bool TryGetObjectName(out string targetName)
        {
            var ray = _camera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
            if (Physics.Raycast(ray, out var hit))
            {
                targetName = hit.collider.name;
                return true;
            }

            targetName = string.Empty;
            return false;
        }

        private async Task RenderAsync()
        {
            _camera.targetTexture = _cameraRenderTexture;
            await Task.Delay(100);
            Graphics.Blit(_camera.targetTexture, _texture);
            var texture2d = _texture.toTexture2D();
            RemoveAlphaUnnecessary(texture2d);
            await WriteAsync(texture2d.EncodeToPNG());
            _camera.targetTexture = null;
        }

        private void RemoveAlphaUnnecessary(Texture2D texture2D)
        {
            var refPixelColor = texture2D.GetPixel(0, 0);
            for (int i = 0; i < texture2D.width; i++)
            {
                for (int j = 0; j < texture2D.height; j++)
                {
                    var color = texture2D.GetPixel(i, j);
                    if (color == refPixelColor)
                        texture2D.SetPixel(i, j, Color.clear);
                }
            }

            texture2D.Apply();
        }

        private async Task WriteAsync(byte[] bytes)
        {
            //var bytes = texture2d.EncodeToPNG();
            await System.IO.File.WriteAllBytesAsync(_objectPath, bytes);
            AssetDatabase.ImportAsset(_objectPath.GetLocalPathFromAbsolute());
            _camera.targetTexture = null;
        }
    }
}