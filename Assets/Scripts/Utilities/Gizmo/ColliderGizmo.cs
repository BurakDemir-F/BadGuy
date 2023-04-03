using System;
using UnityEngine;

namespace Utilities.Gizmo
{
    public class ColliderGizmo : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Color _gizmoColor;
        private void OnValidate()
        {
            _collider = GetComponent<BoxCollider>();
        }
        
        private void OnDrawGizmos()
        {
            if(!_collider)
                return;

            var size = _collider.size;
            var center = _collider.center;
            var myTransform = transform;
            Quaternion orientation = myTransform.rotation;

            Gizmos.color = _gizmoColor;

            // Draw a wireframe cube around the Box Collider
            Gizmos.matrix = Matrix4x4.TRS(myTransform.position, orientation, size);
            Gizmos.DrawWireCube(center, Vector3.one);
        }

    }
}