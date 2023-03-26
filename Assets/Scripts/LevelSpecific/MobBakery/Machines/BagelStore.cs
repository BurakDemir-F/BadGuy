using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines
{
    public class BagelStore : BaseMachine
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
            
            CurrentHolder.Give(ingredientSo);
        }
    }
}