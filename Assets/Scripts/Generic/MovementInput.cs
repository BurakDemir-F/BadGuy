using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Generic
{
    public class MovementInput : MonoBehaviour, Inputs.IPlayerActions
    {
        [SerializeField] private float _inputSmoothSpeed = .2f;
        private Vector2 _refSmoothValue;
        private Vector2 _currentInput;
        
        private Inputs _inputs;
        private Inputs.PlayerActions _playerActions;
        public Vector2 MoveVector { get; private set; }
        public Vector2 CurrentMoveVector => _currentInput;
        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }

        protected virtual void Start()
        {
            _inputs = new Inputs();
            _playerActions = new Inputs.PlayerActions(_inputs);
            _playerActions.SetCallbacks(this);
            _inputs.Player.Enable();
        }

        protected virtual void OnDestroy()
        {
            _inputs.Player.Disable();
            _inputs.Dispose();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>();
        }

        public void OnHorizontalAxis(InputAction.CallbackContext context)
        {
            HorizontalAxis = context.ReadValue<float>();
        }

        public void OnVerticalAxis(InputAction.CallbackContext context)
        {
            VerticalAxis = context.ReadValue<float>();
        }

        protected virtual void Update()
        {
            if(MoveVector.x == 0 && MoveVector.y == 0)
                return;

            _currentInput = Vector2.SmoothDamp(_currentInput, MoveVector, ref _refSmoothValue, _inputSmoothSpeed);
        }
    }
}
