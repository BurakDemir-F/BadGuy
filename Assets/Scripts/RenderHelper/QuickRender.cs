using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace RenderHelper
{
    public class QuickRender : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RenderTexture _texture;
        private void Awake()
        {
            _camera.targetTexture = new RenderTexture(300, 300, GraphicsFormat.D32_SFloat, GraphicsFormat.R16_SFloat);
            _texture = new RenderTexture(300, 300, GraphicsFormat.D32_SFloat, GraphicsFormat.R16_SFloat);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            Render();
        }

        private void Render()
        {
            Graphics.Blit(_camera.targetTexture,_texture);
            var texture2d = _texture.toTexture2D();
            var bytes = texture2d.EncodeToPNG();
            
        }
    }
    
    public static class ExtensionMethod
    {
        public static Texture2D toTexture2D(this RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
            var old_rt = RenderTexture.active;
            RenderTexture.active = rTex;

            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();

            RenderTexture.active = old_rt;
            return tex;
        }
    }
}