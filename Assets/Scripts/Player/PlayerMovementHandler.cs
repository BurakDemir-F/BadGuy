using Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementHandler : CameraBasedMovementHandler
    {
        private CharacterController _character;

        protected override void Start()
        {
            base.Start();
            _character = GetComponent<CharacterController>();
        }

        protected override void Move()
        {
            _character.Move(MoveDirectionOnXZ * (Time.deltaTime * Speed));
        }

        protected override void ApplyGravity()
        {
            if(_character.isGrounded)
            {
                MoveDirectionOnXZ.y = 0f;
                return;
            }

            MoveDirectionOnXZ.y = -FallSpeed * Gravity * Time.deltaTime;
        }
    }
}