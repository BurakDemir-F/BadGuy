using System;
using System.Collections.Generic;
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
        
        private void OnEnable()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.Disable();
            }
        }

        private void OnDestroy()
        {
            foreach (var inputGameAction in GameActions)
            {
                inputGameAction.InputAction.performed -= OnActionPerformed;
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