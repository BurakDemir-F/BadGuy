using UnityEngine;

namespace LevelSpecific.MobBakery.IngredientSystem
{
    // Food atom

    [CreateAssetMenu(menuName = "Data/Config/Ingredient", fileName = "Ingredient", order = 0)]
    public class IngredientSO : ScriptableObject
    {
        public Ingredient Item;
    }

    [System.Serializable]
    public class Ingredient
    {
        public IngredientType Type;
        public string Name;
        public GameObject Prefab;
        public GameObject UIObject;
    }

    public enum IngredientType
    {
        None,
        CoffeeCup,
        Coffee,
        Bagel,
        Loaf,
        Concha
    }
}