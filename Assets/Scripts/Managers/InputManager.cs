using System.Collections.Generic;
using General;
using InputRelated;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour, IItemHolder<IInputControlProvider>,IInputManager
    {
        private Dictionary<InputType, IInputControlProvider> _inputMap;
        private InputType _activeInput;
        private bool _isInitialized;
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if(_isInitialized)
                return;
            _isInitialized = true;
            _inputMap = new Dictionary<InputType, IInputControlProvider>();
        }

        public void Add(IInputControlProvider item)
        {
            Init();
            if (_inputMap.ContainsKey(item.InputType))
                return;
            _inputMap.Add(item.InputType,item);
        }

        public void Remove(IInputControlProvider item)
        {
            Init();
            if (!_inputMap.ContainsKey(item.InputType))
                return;
            _inputMap.Remove(item.InputType);
        }

        public void ActivateInput(InputType type)
        {
            if(!_inputMap.ContainsKey(type))
                return;
            _inputMap[type].EnableInput();
        }

        public void DeactivateInput(InputType type)
        {
            if(!_inputMap.ContainsKey(type))
                return;
            _inputMap[type].DisableInput();
        }
    }

    public interface IInputManager
    {
        void ActivateInput(InputType type);
        void DeactivateInput(InputType type);
    }
}