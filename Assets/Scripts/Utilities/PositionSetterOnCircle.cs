using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class PositionSetterOnCircle : MonoBehaviour
    {
        public List<Transform> path;
        public float radius;
        public Direction plane;
        public Vector3 centerPos;
        public float gizmoScale = 1f;
        public bool drawGizmo = false;
        public bool isLocalSpace = false;

        
        [ContextMenu("Set Positions")]
        public void SetPositions()
        {
            if (isLocalSpace)
                centerPos = transform.position + centerPos;
            
            SetPositions(centerPos,radius,path);
        }
        
        public void SetPositions(Vector3 centerPos,float radius,List<Transform> transforms,Direction plane = Direction.XZ)
        {
            var angle = 360f / transforms.Count;
            for (int i = 0; i < transforms.Count; i++)
            {
                var currentAngle = angle * i;
                if(plane == Direction.XY) transforms[i].PlaceOnCircleXY(centerPos, currentAngle, radius);
                if(plane == Direction.XZ) transforms[i].PlaceOnCircleXZ(centerPos, currentAngle, radius);
            }
        }

        private void OnDrawGizmos()
        {
            if(!drawGizmo)
                return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(centerPos, gizmoScale);

            if (path == null || path.Count == 0) return;

            for (int i = 0; i < path.Count; i++)
            {
                if (i < path.Count - 1) Gizmos.DrawLine(path[i].position, path[i + 1].position);
                if (i == path.Count - 1) Gizmos.DrawLine(path[0].position, path[^1].position);
            }
        }
    }

    public enum Direction
    {
        None,
        XY,
        XZ
    }
}