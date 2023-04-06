using Sequence.System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Sequence.UIObjectsCamera
{
    public class EnableUIObjectsCamera : SequenceNode
    {
        [SerializeField] private UniversalAdditionalCameraData _baseCameraData;
        [SerializeField] private Camera _uiObjectsCam;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            EnableCamera();
            SequenceNodeCompleted?.Invoke();
        }

        public void EnableCamera()
        {
            Debug.Log("ui object camera enabled.");
            _baseCameraData.cameraStack.Add(_uiObjectsCam);
        }
    }
}