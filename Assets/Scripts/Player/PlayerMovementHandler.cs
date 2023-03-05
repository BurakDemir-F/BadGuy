using System;
using Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementHandler : XZMovementHandler
    {
        private CharacterController _character;

        protected override void Start()
        {
            base.Start();
            _character = GetComponent<CharacterController>();
        }

        protected override void OnMovementDirectionChanged()
        {
            var collisionFlags = _character.Move(MoveDirectionOnXZ * (Time.deltaTime * Speed));
            Rotate();
        }
    }
}