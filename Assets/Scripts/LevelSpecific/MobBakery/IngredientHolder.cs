using System;
using System.Collections.Generic;
using Generic.ShowSystem;
using LevelSpecific.MobBakery.IngredientSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSpecific.MobBakery
{
    public class IngredientHolder : MonoBehaviour               
    {
        [SerializeField] private InputButton _inputButton;
        private Ingredient _ingredient;

        private Dictionary<IngredientType, GameObject> _ingredients;

        private bool _hasIngredient;
        private bool _isInitialized;
        public bool HasIngredient => _hasIngredient;

        private void Start()
        {
            _inputButton.gameObject.SetActive(false);
        }

        public void Init()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            _ingredients = new Dictionary<IngredientType, GameObject>();
            _ingredient = new Ingredient();
        }
        
        public void Give(IngredientSO ingredientSo)
        {
            Init();
            _inputButton.gameObject.SetActive(true);
            _ingredient.Type = ingredientSo.Item.Type;
            _ingredient.Name = ingredientSo.Item.Name;
            _ingredient.Prefab = CreateAndStoreVisual(ingredientSo);
            _ingredient.Prefab.transform.SetParent(_inputButton.transform);
            _inputButton.SetButtonData(this._ingredient.Prefab,ingredientSo);
            _hasIngredient = true;
        }

        public Ingredient Take()
        {
            _inputButton.gameObject.SetActive(false);
            _ingredient.Prefab.gameObject.SetActive(false);
            return _ingredient;
        }

        private GameObject CreateAndStoreVisual(IngredientSO ingredientSo)
        {
            var key = ingredientSo.Item.Type;
            if (_ingredients.ContainsKey(key))
                return _ingredients[key];

            var newObj = Instantiate(ingredientSo.Item.UIObject);
            _ingredients.Add(key,newObj);
            return newObj;
        }
    }
}