using System;
using System.Collections;
using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines
{
    public class TimeBasedMachine : BaseMachine
    {
        [SerializeField] protected FoodMachineTimeUI _timeUI;
        protected bool _isProducing;
        protected bool _hasProduced;

        public override void Interact(Collider col)
        {
            base.Interact(col);

            if (_isProducing)
                return;

            if (_hasProduced)
            {
                ActivateButtonAndInput();
                return;
            }

            var hasItem = CurrentHolder.HasIngredient;

            if (!hasItem)
            {   
                ActivatePassiveButtons();
                return;
            }

            var ingredient = CurrentHolder.Ingredient;
            if (!Ingredients.ContainsKey(ingredient.Type))
            {
                ActivatePassiveButtons();
                return;
            }

            CurrentHolder.Release();
            _isProducing = true;
            var duration = Foods.MachineProcessDuration;
            StartCoroutine(TimerCor(duration,ActivateButtonAndInput));
        }

        protected IEnumerator TimerCor(float duration, Action endCallback)
        {
            _timeUI.ActivateUI();
            _timeUI.ResetUI();
            var timer = 0f;
            while (timer < duration)
            {
                _timeUI.UpdateUI(timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            _timeUI.UpdateUI(1f);
            _timeUI.DeactivateUI();

            //ActivateButtonAndInput();
            _isProducing = false;
            _hasProduced = true;
            endCallback?.Invoke();
        }

        protected override void OpenMachine(Collider col)
        {
            OpenDoor();
            CurrentHolder = col.GetComponent<IngredientHolder>();
        }

        private void ActivateButtonAndInput()
        {
            _machineUI.ActivateButtons();
        }

        private void ActivatePassiveButtons()
        {
            _machineUI.ActivatePassiveButtons();
        }

        protected override void OnIngredientSelected(IngredientSO ingredientSo)
        {
            var hasAnyItem = CurrentHolder.HasIngredient;
            if(hasAnyItem)
                return;
            
            CurrentHolder.Hold(ingredientSo);
            _machineUI.DeactivateButtons();
            _hasProduced = false;
        }
    }
}