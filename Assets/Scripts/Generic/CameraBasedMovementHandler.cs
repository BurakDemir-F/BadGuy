using AYellowpaper;
using Generic.Providers;
using UnityEngine;

namespace Generic
{
    public class CameraBasedMovementHandler : XZMovementHandler
    {
        [SerializeField] private InterfaceReference<IObjectProvider<Transform>> _transformProvider;

        // camera relative movement.
        protected override Vector3 GetMovementDirection()
        {
            var camTransform = _transformProvider.Value.Get();
            var camDirectionForward = camTransform.forward;
            var camDirectionRight = camTransform.right;

            camDirectionForward.y = 0f;
            camDirectionRight.y = 0f;
            
            camDirectionForward.Normalize();
            camDirectionRight.Normalize();

            var movementDirection = camDirectionForward * CurrentMoveVector.y + camDirectionRight * CurrentMoveVector.x;
            return movementDirection;
        }
    }
}