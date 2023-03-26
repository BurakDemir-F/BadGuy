using System;
using Generic;
using Injector;
using InputRelated;
using InteractableArea;
using LevelSpecific.MobBakery.IngredientSystem;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSpecific.MobBakery.Machines
{
    public abstract class BaseMachine : MonoBehaviour,IInteractable
    {
        [SerializeField] protected CD_FoodMachine Foods;
        [SerializeField] private FoodMachineUI _machineUI;
        private Door _door;

        [InjectReference]
        public IInputManager InputManager { get; set; }

        protected IngredientHolder CurrentHolder;
        protected bool IsGiver;
        protected bool IsRecipeMaker;
        
        private void Start()
        {
            _door = GetComponent<Door>();
            _machineUI.IngredientSelected += OnIngredientSelected;

            IsGiver = Foods.HasIngredients;
            IsRecipeMaker = Foods.HasRecipes;
        }

        private void OnDestroy()
        {
            _machineUI.IngredientSelected -= OnIngredientSelected;
        }

        public virtual void Interact(Collider col)
        {
            InputManager.ActivateInput(InputType.InGameUI);
            _door.Open();
            _machineUI.ActivateButtons();
            CurrentHolder = col.GetComponent<IngredientHolder>();
        }

        public virtual void InteractEnd(Collider col)
        {
            InputManager.DeactivateInput(InputType.InGameUI);
            _door.Close();
            _machineUI.DeactivateButtons();
            CurrentHolder = null;
        }

        protected abstract void OnIngredientSelected(IngredientSO ingredientSo);
    }
}