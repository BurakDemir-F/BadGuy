using System;
using System.Collections.Generic;
using General;
using Injector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputRelated
{
    public class InputReceiver<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] protected List<InputGameAction<TEnum>> GameActions;
        public event Action<TEnum> GameActionPerformed;

        private Dictionary<InputAction, TEnum> _actionTypeDict;

        private void Awake()
        {
            _actionTypeDict = new Dictionary<InputAction, TEnum>();

            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.performed += OnActionPerformed;
                _actionTypeDict.Add(inputGameAction.InputAction,inputGameAction.GameActionType);
            }
        }
        
        private void OnActionPerformed(InputAction.CallbackContext obj)
        {
            var gameActionType = _actionTypeDict[obj.action];
            GameActionPerformed?.Invoke(gameActionType);
            OnGameActionPerformed(gameActionType);
        }

        protected virtual void OnGameActionPerformed(TEnum gameActionType)
        {
            
        }
        
        protected virtual void OnEnable()
        {
            EnableInput();
        }

        protected virtual void OnDisable()
        {
            DisableInput();
        }

        protected virtual void OnDestroy()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.performed -= OnActionPerformed;
            }
        }

        public void EnableInput()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.Enable();
            }
        }

        public void DisableInput()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.Disable();
            }
        }
    }

    [Serializable]
    public class InputGameAction<TEnum> where TEnum : Enum
    {
        public InputAction InputAction;
        public TEnum GameActionType;
    }
}