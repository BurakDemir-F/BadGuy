using UnityEngine;

namespace Utilities.Gizmo
{
    public class SimpleGizmoDrawer : MonoBehaviour
    {
        [SerializeField] protected bool _isActivated;
        [SerializeField] protected bool _showInRuntime;
        [SerializeField] protected Color _color = Color.green;
        [SerializeField] protected float _size = .1f;
        [SerializeField] protected bool _drawWire;
        [SerializeField] protected Shape _gizmoShape;

        public void ActivateGizmo()
        {
            _isActivated = true;
            _showInRuntime = true;
        }
        
        private void OnDrawGizmos()
        {
            if (!_isActivated)
                return;

            var isAllowedToDraw = _showInRuntime || !Application.isPlaying;
            if(!isAllowedToDraw)
                return;
            
            Gizmos.color = _color;
            Draw();
        }

        protected virtual void Draw()
        {
            var position = transform.position;
            DrawPoint(position);
        }

        protected virtual void DrawPoint(Vector3 position)
        {
            if (_gizmoShape == Shape.Sphere)
            {
                if(!_drawWire)
                    Gizmos.DrawSphere(position,_size);
                else
                    Gizmos.DrawWireSphere(position,_size);
            }
            else
            {
                if(!_drawWire)
                    Gizmos.DrawCube(position,Vector3.one * _size);
                else
                    Gizmos.DrawWireCube(position,Vector3.one * _size);
            }
        }
    }

    public enum Shape
    {
        Sphere,Cube
    }
}