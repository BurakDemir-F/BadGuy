using System;
using System.Collections.Generic;
using Generic.ShowSystem;
using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelSpecific.MobBakery
{
    public class FoodMachineUI : MonoBehaviour
    {
        [SerializeField] private CD_FoodMachine _foods;
        [SerializeField] private UISelectionSystem _uiSystem;
        [SerializeField] private List<InputButton> _inputButtons;
        private List<InputButton> _buttonsInUse;

        public event Action<IngredientSO> IngredientSelected; 

        private void Start()
        {
            CreateButtons();
            _uiSystem.Init(_buttonsInUse);
            
            foreach (var inputButton in _inputButtons)
            {
                inputButton.gameObject.SetActive(false);
            }

            foreach (var button in _buttonsInUse)
            {
                button.gameObject.SetActive(true);
                button.Deactivate();
                button.ButtonPressed += OnButtonSelected;
            }
        }

        private void OnDestroy()
        {
            foreach (var button in _buttonsInUse)
            {
                button.ButtonPressed -= OnButtonSelected;
            }
        }

        private void CreateButtons()
        {
            Debug.Assert(_foods.Ingredients.Count < _inputButtons.Count,
                "something wrong with ingredient and button count!");
            
            _buttonsInUse = new List<InputButton>();

            for (var i = 0; i < _foods.Ingredients.Count; i++)
            {
                var ingredient = _foods.Ingredients[i];
                var newUIObj = Instantiate(ingredient.Item.UIObject);
                var button = _inputButtons[i];
                button.SetButtonData(newUIObj,ingredient);
                _buttonsInUse.Add(button);
            }
        }

        public void ActivateButtons()
        {
            foreach (var inputButton in _buttonsInUse)
            {
                inputButton.Activate();
            }
            
            _uiSystem.EnableInput();
        }

        public void DeactivateButtons()
        {
            foreach (var inputButton in _buttonsInUse)
            {
                inputButton.Deactivate();
            }
            _uiSystem.DisableInput();
        }

        private void OnButtonSelected(Object obj)
        {
            var ingredient = obj as IngredientSO;
            IngredientSelected?.Invoke(ingredient);
        }
    }
}