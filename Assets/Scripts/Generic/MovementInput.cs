using System;
using General;
using Injector;
using InputRelated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Generic
{
    public class MovementInput : MonoBehaviour, Inputs.IPlayerActions,IInputControlProvider
    {
        [SerializeField] private float _inputSmoothSpeed = .2f;
        private Vector2 _refSmoothValue;
        private Vector2 _currentInput;
        
        private Inputs _inputs;
        private Inputs.PlayerActions _playerActions;
        public Vector2 MoveVector { get; private set; }
        public Vector2 CurrentMoveVector => _currentInput;
        public float VerticalAxis { get; private set; }
        public InputType InputType => InputType.Movement;
        
        [InjectReference]
        public IItemHolder<IInputControlProvider> _inputHolder { get; set; }

        protected virtual void Start()
        {
            _inputHolder.Add(this);
            _inputs = new Inputs();
            _playerActions = new Inputs.PlayerActions(_inputs);
            _playerActions.SetCallbacks(this);
            _inputs.Player.Enable();
        }

        protected virtual void OnDestroy()
        {
            _inputHolder.Remove(this);
            _inputs.Player.Disable();
            _inputs.Dispose();
        }

        public virtual void EnableInputs()
        {
            _inputs.Player.Enable();
            _inputs.Player.Movement.Enable();
            _inputs.Player.Look.Enable();
        }

        public virtual void DisableInputs()
        {
            _inputs.Player.Disable();
            _inputs.Player.Look.Disable();
            _inputs.Player.Movement.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
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
            
            if (MoveVector.x == 0f)
                _currentInput.x = 0f;

            if (MoveVector.y == 0f)
                _currentInput.y = 0f;
        }

        public void EnableInput()
        {
            EnableInputs();
        }

        public void DisableInput()
        {
            DisableInputs();
        }

    }
}
