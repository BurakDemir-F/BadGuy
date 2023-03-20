using Cinemachine;
using Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementHandler : CameraBasedMovementHandler
    {
        private CharacterController _character;
        [SerializeField] private CinemachineFreeLook _freeLookCam;

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

        public override void EnableInputs()
        {
            base.EnableInputs();
            _freeLookCam.GetComponent<CinemachineInputProvider>().enabled = true;
        }

        public override void DisableInputs()
        {
            base.DisableInputs();
            _freeLookCam.GetComponent<CinemachineInputProvider>().enabled = false;
        }
    }
}