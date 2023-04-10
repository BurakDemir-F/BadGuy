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
        [SerializeField] private PlayerEffects _effects;

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

        private void PlayDieEffects()
        {
            _effects.PlayDieEffect();
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

        public void Die()
        {
            _movementHandler.DisableInputs();
            _animator.Animate(AnimType.Death);
            PlayDieEffects();
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