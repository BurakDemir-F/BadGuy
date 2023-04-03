using System;
using System.Collections.Generic;
using DG.Tweening;
using Generic;
using Generic.Items;
using Injector;
using InputRelated;
using InteractableArea;
using LevelSpecific.MobBakery.IngredientSystem;
using Managers;
using UnityEngine;
using Utilities;

namespace LevelSpecific.MobBakery.Machines
{
    public abstract class BaseMachine : MonoBehaviour, IInteractable
    {
        [SerializeField] protected CD_FoodMachine Foods;
        [SerializeField] protected FoodMachineUI _machineUI;
        [SerializeField] protected List<FlyObjectBehaviour> _indicators;
        private Door _door;
        protected IngredientHolder CurrentHolder;
        protected Dictionary<IngredientType, Ingredient> Ingredients;
        
        private void Start()
        {
            _door = GetComponent<Door>();
            _machineUI.IngredientSelected += OnIngredientSelected;
            PickIngredients();
            ChangeIndicatorActivationStatus(true);
        }

        protected void ChangeIndicatorActivationStatus(bool status)
        {
            foreach (var item in _indicators)
            {
                if(status)
                    item.EnableIndicator();
                else
                    item.DisableIndicator();
            }
        }

        private void PickIngredients()
        {
            Ingredients = new Dictionary<IngredientType, Ingredient>();
            foreach (var foodsIngredient in Foods.Ingredients)
            {
                Ingredients.Add(foodsIngredient.Item.Type,foodsIngredient.Item);
            }
        }

        private void OnDestroy()
        {
            _machineUI.IngredientSelected -= OnIngredientSelected;
        }

        public virtual void Interact(Collider col)
        {
            OpenMachine(col);
        }

        public virtual void InteractEnd(Collider col)
        {
           CloseMachine();
        }

        protected virtual void OpenMachine(Collider col)
        {
            OpenDoor();
            _machineUI.ActivateButtons();
            CurrentHolder = col.GetComponent<IngredientHolder>();
        }

        protected virtual void CloseMachine()
        {
            CloseDoor();
            _machineUI.DeactivateButtons();
            CurrentHolder = null;
        }

        protected void OpenDoor()
        {
            if(!Foods.HasDoor)
                return;
            _door.Open();
        }

        protected void CloseDoor()
        {
            if(!Foods.HasDoor)
                return;
            _door.Close();
        }

        protected virtual void OnIngredientSelected(IngredientSO ingredientSo){}
    }
}