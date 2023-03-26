using System.Collections.Generic;
using UnityEngine;

namespace LevelSpecific.MobBakery.IngredientSystem
{
    
    [CreateAssetMenu(menuName = "Data/Config/FoodProduct", fileName = "FoodProduct", order = 0)]
    public class FoodProduct : IngredientSO
    {
        public List<IngredientSO> Ingredients;
    }
}