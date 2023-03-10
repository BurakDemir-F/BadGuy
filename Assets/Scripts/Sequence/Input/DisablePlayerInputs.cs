using Generic;
using Sequence.System;
using UnityEngine;

namespace Sequence.Input
{
    public class DisablePlayerInputs : SequenceNode
    {
        [SerializeField] private MovementInput _movementInput;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            PlayNode();
        }

        private void PlayNode()
        {
            _movementInput.DisableInputs();
            isNodeCompleted = true;
            SequenceNodeCompleted?.Invoke();
        }
    }
}