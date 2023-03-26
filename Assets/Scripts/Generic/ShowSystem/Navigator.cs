using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generic.ShowSystem
{
    public abstract class Navigator : MonoBehaviour
    {
        protected List<InputButton> Buttons;
        protected int CurrentButtonIndex;
        protected InputButton CurrentButton;
        public void Init(List<InputButton> buttons)
        {
            Buttons = buttons;
            CurrentButton = Buttons[0];
            CurrentButton.HighLight();

            for (var i = 1; i < Buttons.Count; i++)
            {
                var inputButton = Buttons[i];
                inputButton.CloseHighlight();
            }
        }

        public abstract InputButton Navigate(SelectionInputType inputType);
        
        protected void MoveForward()
        {
            CurrentButton.CloseHighlight();
            CurrentButtonIndex = (CurrentButtonIndex + 1) % Buttons.Count;
            CurrentButton = Buttons[CurrentButtonIndex];
            CurrentButton.HighLight();
        }

        protected void MoveBackward()
        {
            CurrentButton.CloseHighlight();
            CurrentButtonIndex = CurrentButtonIndex == 0 ? Buttons.Count - 1 : CurrentButtonIndex - 1;
            CurrentButton = Buttons[CurrentButtonIndex];
            CurrentButton.HighLight();
        }

        public InputButton GetCurrentButton()
        {
            return CurrentButton;
        }
    }
}