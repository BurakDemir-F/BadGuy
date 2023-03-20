using UnityEngine;

namespace Generic.Items.SO
{
    [CreateAssetMenu(menuName = "Data/Config/ThrowableItem", fileName = "ThrowableItem", order = 0)]
    public class CD_ThrowableItem : ScriptableObject, IThrowableItemDataProvider
    {
        [SerializeField] private float _throwForce;
        [SerializeField] private float _mass;
        [SerializeField] private float _shakeMaxDistance;
        [SerializeField] private float _maxShakeRotation;
        [SerializeField] private float _resetRotationDuration;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _waifForStartHolding;
        

        public float ThrowForce => _throwForce;
        public float Mass => _mass;
        public float ShakeMinDistance => _shakeMaxDistance;
        public float WaitForStartHolding => _waifForStartHolding;
        public float ShakeMaxDistance => _shakeMaxDistance;
        public float MaxShakeRotation => _maxShakeRotation;
        public float ResetRotationDuration => _resetRotationDuration;
        public float RotateSpeed => _rotateSpeed;
    }
}