using System;
using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Generic.ShowSystem
{
    public class InputButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private GameObject _buttonVisual;
        [SerializeField] private Image _highLightImage;
        [SerializeField] private Object _data;
        [SerializeField] private Transform _visualTransform;
        public event Action<Object> ButtonPressed;

        public void HighLight()
        {
            _highLightImage.gameObject.SetActive(true);
        }

        public void CloseHighlight()
        {
            _highLightImage.gameObject.SetActive(false);
        }

        public void Click()
        {
            _button.onClick?.Invoke();
            ButtonPressed?.Invoke(_data);
        }

        public void SetButtonData(GameObject visual, Object data)
        {
            _buttonVisual = visual;
            _buttonVisual.transform.position = _visualTransform.position;
            _data = data;
        }

        public void Activate()
        {
            SetActivationStatus(true);
        }

        public void Deactivate()
        {
            SetActivationStatus(false);
        }

        private void SetActivationStatus(bool status)
        {
            _buttonVisual.SetActive(status);
            _buttonImage.gameObject.SetActive(status);
        }

        public void CloseImage()
        {
            _buttonImage.gameObject.SetActive(false);
        }
    }
}