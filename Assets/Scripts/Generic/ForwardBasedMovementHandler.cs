using UnityEngine;

namespace Generic
{
    public class ForwardBasedMovementHandler : XZMovementHandler
    {
        protected override void OnMovementDirectionChanged()
        {
            Debug.Log($"movement direction: {MoveDirectionOnXZ}");

            if(MoveDirectionOnXZ.z != 0)
                Move();
            
            Rotate();
        }

        protected override Vector3 GetMovementDirection()
        {
            var myTransform = transform;
            var forward = myTransform.forward;
            var right = myTransform.right;

            forward.y = 0f;
            right.y = 0f;
            
            forward.Normalize();
            right.Normalize();

            var movementDirection = forward * CurrentMoveVector.y + right * CurrentMoveVector.x;
            return movementDirection;
        }
    }
}