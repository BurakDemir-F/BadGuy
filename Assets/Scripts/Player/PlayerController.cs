using System;
using Generic;
using Generic.Animation;
using UnityEngine;
using Utilities;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PlayerMovementHandler))]
    public class PlayerController : MonoBehaviour, IInteractable,IMissionGoalInformer
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

        public void Interact(Collider col)
        {
            if (col.CompareTag("Npc"))
            {
                //_character.enabled = false;
                _movementHandler.enabled = false;
                var posY = transform.position.y;
                transform.LookAt(col.transform.position.SetY(posY));
                _animator.Animate(AnimType.Catched);
            }
        }

        public void InteractEnd(Collider col)
        {
        }

        public bool GoalAccomplished { get; private set; }
        public void SetGoalResult(bool isAccomplished)
        {
            GoalAccomplished = isAccomplished;
        }
    }

    public interface IMissionGoalInformer
    {
        bool GoalAccomplished { get; }
        void SetGoalResult(bool isAccomplished);
    }
}