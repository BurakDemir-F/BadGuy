using System;
using UnityEngine;

namespace Utilities
{
    public class BoxColliderGizmo : MonoBehaviour
    {
        private BoxCollider _collider;

        [Header("MainArea")]
        [SerializeField] private Color gizmoColor = Color.green;
        [SerializeField] private bool drawSolid = false;

        private void OnValidate()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
            if(_collider == null) return;

            var size = _collider.size;
            var pos = transform.position;
            Gizmos.color = gizmoColor;
            
            if(drawSolid)
                Gizmos.DrawCube(pos,size);
            else
                Gizmos.DrawWireCube(pos,size);
        }
    }
}