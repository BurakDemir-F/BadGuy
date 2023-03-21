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
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _inputMap = new Dictionary<InputType, IInputControlProvider>();
        }

        public void Add(IInputControlProvider item)
        {
            if (_inputMap.ContainsKey(item.InputType))
                return;
            _inputMap.Add(item.InputType,item);
        }

        public void Remove(IInputControlProvider item)
        {
            
            if (!_inputMap.ContainsKey(item.InputType))
                return;
            _inputMap.Remove(item.InputType);
        }

        public void ActivateInput(InputType type)
        {
            _inputMap[type].EnableInput();
        }

        public void DeactivateInput(InputType type)
        {
            _inputMap[type].DisableInput();
        }
    }

    public interface IInputManager
    {
        void ActivateInput(InputType type);
        void DeactivateInput(InputType type);
    }
}