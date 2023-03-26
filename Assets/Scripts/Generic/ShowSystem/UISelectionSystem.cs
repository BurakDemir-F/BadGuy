using System.Collections.Generic;
using General;
using Injector;
using InputRelated;
using UnityEngine;

namespace Generic.ShowSystem
{
    public class UISelectionSystem : InputReceiver<SelectionInputType>, IInputControlProvider
    {
        [SerializeField] private Navigator _navigator;
        private List<InputButton> _buttons;
        public InputType InputType => InputType.InGameUI;
        [InjectReference]
        public IItemHolder<IInputControlProvider> InputHolder { get; set; }

        protected virtual void Start()
        {
            InputHolder.Add(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputHolder.Remove(this);
        }

        public void Init(List<InputButton> buttons)
        {
            _buttons = buttons;
            _navigator.Init(_buttons);
        }

        protected override void OnGameActionPerformed(SelectionInputType gameActionType)
        {
            if (_buttons.Count == 0)
                return;

            if (_buttons.Count == 1 && gameActionType != SelectionInputType.Select)
                return;

            if (gameActionType == SelectionInputType.Select)
            {
                _navigator.GetCurrentButton().Click();
                return;
            }

            _navigator.Navigate(gameActionType);
        }
    }

    public enum SelectionInputType
    {
        None,
        Forward,
        Backward,
        Left,
        Right,
        Select
    }
}