using UnityEngine;

namespace Generic.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] protected ItemIndicator _indicator;
        protected Rigidbody _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            DisableIndicator();
        }

        protected void RigidbodySetDynamic(bool isDynamic)
        {
            _rigidbody.isKinematic = !isDynamic;
            _rigidbody.useGravity = isDynamic;
        }

        protected void DisableIndicator()
        {
            _indicator.DisableIndicator();
        }
    }
}