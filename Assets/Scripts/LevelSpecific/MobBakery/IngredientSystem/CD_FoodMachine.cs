using System.Collections.Generic;
using UnityEngine;

namespace LevelSpecific.MobBakery.IngredientSystem
{
    [CreateAssetMenu(fileName = "CD_FoodMachine", menuName = "Data/Config/FoodMachine", order = 0)]
    public class CD_FoodMachine : ScriptableObject
    {
        // already have ingredients
        public List<IngredientSO> Ingredients;
        
        //Recipes
        public List<FoodProduct> Products;
        public bool IsTimeMachine;
        public float MachineProcessDuration;
        public bool HasDoor;

        public bool HasIngredients => Ingredients.Count > 0;
        public bool HasRecipes => Products.Count > 0;

    }
}