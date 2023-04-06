using Sequence.System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Sequence.UIObjectsCamera
{
    public class DisableUIObjectsCameraNode : SequenceNode
    {
        [SerializeField] private UniversalAdditionalCameraData _baseCameraData;
        [SerializeField] private Camera _uiObjectsCam;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            DisableCamera();
            SequenceNodeCompleted?.Invoke();
        }

        public void DisableCamera()
        {
            _baseCameraData.cameraStack.Remove(_uiObjectsCam);
        }
    }
}