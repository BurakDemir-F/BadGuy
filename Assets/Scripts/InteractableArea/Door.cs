using System;
using DG.Tweening;
using UnityEngine;

namespace InteractableArea
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform _rotationRoot;
        [SerializeField] private float _rotationAngle;
        [SerializeField] private bool _rotateOnY = true;
        [SerializeField] private bool _rotateOnX;

        private Tween _openTween;
        private Tween _closeTween;

        private Quaternion _openRotation;
        private Quaternion _closeRotation;
        private Vector3 _rotationVec;
        

        private void Start()
        {
            _rotationVec = new Vector3(_rotateOnX ? _rotationAngle : 0f, _rotateOnY ? _rotationAngle : 0f, 0f);
            _closeRotation = _rotationRoot.localRotation;
            _openRotation = Quaternion.Euler(_closeRotation.eulerAngles + _rotationVec);
        }

        [ContextMenu("Open")]
        public void Open()
        {
            _openTween?.Kill();
            _closeTween?.Kill();

            _rotationRoot.localRotation = _closeRotation;
            _openTween = _rotationRoot.DOLocalRotateQuaternion(_openRotation, .5f);
        }

        [ContextMenu("Close")]
        public void Close()
        {
            _openTween?.Kill();
            _closeTween?.Kill();

            _rotationRoot.localRotation = _openRotation;
            _openTween = _rotationRoot.DOLocalRotateQuaternion(_closeRotation, .5f);
        }
    }
}