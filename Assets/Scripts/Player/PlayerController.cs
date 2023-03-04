using System;
using Generic.Animation;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PlayerMovementHandler))]
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _character;
        private CharacterAnimator _animator;
        private PlayerMovementHandler _movementHandler;

        private void Start()
        {
            _character = GetComponent<CharacterController>();
            _animator = GetComponent<CharacterAnimator>();
            _movementHandler = GetComponent<PlayerMovementHandler>();

            _animator.Animate(AnimType.Idle);
            _movementHandler.MovementStarted += OnMovementStarted;
            _movementHandler.MovementEnd += OnMovementEnd;
        }

        private void OnDestroy()
        {
            _movementHandler.MovementStarted -= OnMovementStarted;
            _movementHandler.MovementEnd -= OnMovementEnd;
        }

        private void OnMovementEnd()
        {
            _animator.Animate(AnimType.Idle);
        }

        private void OnMovementStarted()
        {
            _animator.Animate(AnimType.Walk);
        }
    }
}