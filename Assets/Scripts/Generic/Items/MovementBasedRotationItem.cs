using System;
using AYellowpaper;
using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Generic.Items
{
    public class MovementBasedRotationItem : BaseItem
    {
        [SerializeField] private InterfaceReference<IThrowableItemDataProvider> _dataProvider;
        [SerializeField] private Transform _baseTransformForRotation;

        public Rigidbody Rigidbody => _rigidbody;
        public IThrowableItemDataProvider Data => _dataProvider.Value;
        private bool _isMoving;
        private Vector3 _oldPos;
        private Transform _myTransform;
        private float _rotateToZeroTime;
        private DG.Tweening.Sequence _bindTransformSequence;

        public void BindTransform(Transform holdTransform)
        {
            RigidbodySetDynamic(false);
            
            DisableIndicator();
            
            _myTransform = transform;
            _baseTransformForRotation.parent = null;
            _myTransform.parent = _baseTransformForRotation;
            _baseTransformForRotation.parent = holdTransform;

            _bindTransformSequence = DOTween.Sequence();
            _bindTransformSequence
                .Append(_baseTransformForRotation.DOLocalMove(Vector3.zero, Data.WaitForStartHolding))
                .Join(_baseTransformForRotation.DOLocalRotate(Vector3.zero, Data.WaitForStartHolding))
                .AppendCallback(() =>
                {
                    _oldPos = _myTransform.position;
                    _isMoving = true;
                });
        }

        public Transform ReleaseTransform()
        {
            RigidbodySetDynamic(true);
            //EnableIndicator();
            _bindTransformSequence.Kill();
            _baseTransformForRotation.SetParent(null);
            var myTransform = transform;
            myTransform.parent = null;
            _baseTransformForRotation.SetParent(myTransform);
            return transform;
        }
        private void Update()
        {
            if (!_isMoving)
                return;

            RotateBasedOnMovement();
            _oldPos = _myTransform.position;
        }

        private void RotateBasedOnMovement()
        {
            var distanceVec = _myTransform.position - _oldPos;
            var distanceVec2 = new Vector2(distanceVec.x, distanceVec.z);

            if (distanceVec2 == Vector2.zero)
            {
                RotateToZero();
                return;
            }
            
            _rotateToZeroTime = 0f;

            distanceVec = distanceVec.SetY(0f);
            var maxAngle = Data.MaxShakeRotation;

            RotateOnZ(distanceVec, maxAngle);
            RotateOnX(distanceVec, maxAngle);

            var angle = _baseTransformForRotation.localEulerAngles;
            var eulerAngles = new EulerAngleHelper(angle, maxAngle);
            _baseTransformForRotation.localEulerAngles = eulerAngles.ClampedVec;
        }
        
        private void RotateToZero()
        {
            if (_rotateToZeroTime >= Data.ResetRotationDuration)
                return;
            
            var currentAngle = _baseTransformForRotation.localEulerAngles;
            var eulerAngles = new EulerAngleHelper(currentAngle, Data.MaxShakeRotation);
            var targetVec = eulerAngles.ZeroVector - currentAngle;
            _baseTransformForRotation.localEulerAngles = PaintCraft.Utils.Tween.ChangeVector(_rotateToZeroTime, currentAngle, targetVec,
                Data.ResetRotationDuration, PaintCraft.Utils.TweenType.EaseOutBack);

            _rotateToZeroTime += Time.deltaTime;
        }

        private void RotateOnZ(Vector3 distanceVec, float maxAngle)
        {
            var ratioX = Mathf.Clamp(distanceVec.x / Data.ShakeMaxDistance, -1f, 1f);
            var angle = -maxAngle * ratioX * Time.deltaTime * Data.RotateSpeed;
            _baseTransformForRotation.transform.Rotate(Vector3.forward, angle,Space.World);
        }

        private void RotateOnX(Vector3 distanceVec, float maxAngle)
        {
            var ratioZ = Mathf.Clamp(distanceVec.z / Data.ShakeMaxDistance, -1f, 1f);
            var angle = maxAngle * ratioZ * Time.deltaTime * Data.RotateSpeed;
            _baseTransformForRotation.transform.Rotate(Vector3.right, angle,Space.World);
        }
    }

    public struct EulerAngleHelper
    {
        private Vector3 _eulerAngle;
        private float _maxAngle;

        public EulerAngleHelper(Vector3 eulerAngle, float maxAngle)
        {
            _eulerAngle = eulerAngle;
            _maxAngle = maxAngle;
        }
        
        public float X => _eulerAngle.x;
        public float Z => _eulerAngle.z;

        public float ClampedX
        {
            get
            {
                var angleX = _eulerAngle.x;
                var isXNegativeAngle = angleX >= 300f;

                var negativeMaxAngle = 360f - _maxAngle;

                var clampedX = Mathf.Clamp(angleX, isXNegativeAngle ? negativeMaxAngle : 0f,
                    isXNegativeAngle ? 360f : _maxAngle);
                return clampedX;
            }
        }
        
        public float ClampedZ
        {
            get
            {
                var angleZ = _eulerAngle.z;
                var isZNegativeAngle = angleZ >= 300f;
                var negativeMaxAngle = 360f - _maxAngle;
                
                var clampedZ = Mathf.Clamp(angleZ, isZNegativeAngle ? negativeMaxAngle : 0f,
                    isZNegativeAngle ? 360f : _maxAngle);
                return clampedZ;
            }
        }

        public Vector3 ClampedVec => new Vector3(ClampedX, 0f, ClampedZ);

        public Vector3 ZeroVector
        {
            get
            {
                var x = ClampedX;
                var z = ClampedZ;

                return new Vector3(x >= 300f ? 360f : 0f, 0f, z >= 300f ? 360f : 0f);
            }
        }
    }

    public interface IThrowableItemDataProvider
    {
        public float ThrowForce { get; }
        public float Mass { get; }
        public float WaitForStartHolding { get; }
        public float ShakeMaxDistance { get; }
        public float MaxShakeRotation { get; }
        public float ResetRotationDuration { get; }
        public float RotateSpeed { get; }
    }
}