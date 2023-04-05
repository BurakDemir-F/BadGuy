using System.Collections.Generic;
using Generic.ShowSystem;
using Injector;
using InputRelated;
using LevelSpecific.MobBakery.IngredientSystem;
using Managers;
using UnityEngine;

namespace LevelSpecific.MobBakery
{
    public class IngredientHolder : MonoBehaviour               
    {
        [SerializeField] private InputButton _inputButton;
        [SerializeField] private Transform _holderTransform;
        private Ingredient _ingredient;
        private Dictionary<IngredientType, GameObject> _ingredients;
        private bool _hasIngredient;
        private bool _isInitialized;
        public bool HasIngredient => _hasIngredient;
        public Ingredient Ingredient => _ingredient;

        private bool _poisonInputActivated;
        
        [InjectReference]
        public IInputManager InputManager { get; set; }

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
            _inputButton.DeactivateButtonImage();
        }
        
        public void Hold(IngredientSO ingredientSo)
        {
            if (gameObject.CompareTag("Player") && (ingredientSo.Item.Type == IngredientType.Loaf || ingredientSo.Item.Type == IngredientType.FriedLoaf))
            {
                InputManager.ActivateInput(InputType.PoisonInput);
                _poisonInputActivated = true;
            }
            Init();
            _inputButton.gameObject.SetActive(true);
            _ingredient.Type = ingredientSo.Item.Type;
            _ingredient.Name = ingredientSo.Item.Name;
            _ingredient.Prefab = CreateAndStoreVisual(ingredientSo);
            _inputButton.SetButtonData(this._ingredient.Prefab,ingredientSo);
            _inputButton.ActivateVisual();
            _hasIngredient = true;
        }

        public Ingredient Release()
        {
            if(_poisonInputActivated)
                InputManager.DeactivateInput(InputType.PoisonInput);

            _inputButton.gameObject.SetActive(false);
            _ingredient.Prefab.gameObject.SetActive(false);
            _hasIngredient = false;
            return _ingredient;
        }

        private GameObject CreateAndStoreVisual(IngredientSO ingredientSo)
        {
            var key = ingredientSo.Item.Type;
            if (_ingredients.ContainsKey(key))
                return _ingredients[key];

            var newObj = Instantiate(ingredientSo.Item.UIObject, _holderTransform, true);
            newObj.transform.localPosition = Vector3.zero;
            _ingredients.Add(key,newObj);
            Debug.Log("player ingredient created.");
            return newObj;
        }
    }
}