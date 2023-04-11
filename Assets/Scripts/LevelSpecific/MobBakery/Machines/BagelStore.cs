using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines
{
    public class BagelStore : FoodStore
    {
        protected override void OnIngredientSelected(IngredientSO ingredientSo)
        {
            base.OnIngredientSelected(ingredientSo);
            _interactionUI.HideUseArrowCaseForSwitch();
        }

        public override void Interact(Collider col)
        {
            base.Interact(col);
            _interactionUI.ShowUseArrowCaseForSwitch();
        }

        public override void InteractEnd(Collider col)
        {
            base.InteractEnd(col);
            _interactionUI.HideUseArrowCaseForSwitch();
        }
    }
}