using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines
{
    public class FoodStore : BaseMachine
    {
        protected override void OnIngredientSelected(IngredientSO ingredientSo)
        {
            if(CurrentHolder == null)
            {
                Debug.Log("something wrong here.");
                return;
            }

            var isHolding = CurrentHolder.HasIngredient;
            if(isHolding) return;
            
            CurrentHolder.Hold(ingredientSo);
            _interactionUI.HidePressFToSelect();
        }

        public override void Interact(Collider col)
        {
            base.Interact(col);
            _interactionUI.ShowPressFToSelect();
        }
    }
}