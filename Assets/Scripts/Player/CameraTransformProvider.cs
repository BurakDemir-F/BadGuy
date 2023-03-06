using System;
using Generic.Camera;
using Generic.Providers;
using UnityEngine;

namespace Player
{
    public class CameraTransformProvider : MonoBehaviour,IObjectProvider<Transform>
    {
        private AreaCameraManager _camManager;

        private void Start()
        {
            _camManager = GetComponent<AreaCameraManager>();
        }
        
        public Transform Get()
        {
            return _camManager.CurrentCameraTransform;
        }
    }
}