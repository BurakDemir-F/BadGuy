using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Generic.ShowSystem
{
    public class InputButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected Image _buttonImage;
        [SerializeField] protected GameObject _buttonVisual;
        [SerializeField] protected Image _highLightImage;
        [SerializeField] protected Object _data;
        [SerializeField] protected Transform _visualTransform;
        public event Action<Object> ButtonPressed;

        public virtual void HighLight()
        {
            _highLightImage.gameObject.SetActive(true);
        }

        public virtual void CloseHighlight()
        {
            _highLightImage.gameObject.SetActive(false);
        }

        public virtual void Click()
        {
            _button.onClick?.Invoke();
            ButtonPressed?.Invoke(_data);
        }

        public virtual void SetButtonData(GameObject visual, Object data)
        {
            _buttonVisual = visual;
            _buttonVisual.transform.position = _visualTransform.position;
            _buttonVisual.SetActive(false);
            _data = data;
        }

        public virtual void Activate()
        {
            SetActivationStatus(true);
        }

        public virtual void Deactivate()
        {
            SetActivationStatus(false);
        }

        protected void SetActivationStatus(bool status)
        {
            SetVisualActivationStatus(status);
            _buttonImage.gameObject.SetActive(status);
        }

        public void ActivateVisual()
        {
            SetVisualActivationStatus(true);
        }

        public void DeactivateVisual()
        {
            SetVisualActivationStatus(false);
        }

        protected void SetVisualActivationStatus(bool status)
        {
            _buttonVisual.gameObject.SetActive(status);
        }

        public void ActivateButtonImage()
        {
            SetButtonImageActivationStatus(true);
        }

        public void DeactivateButtonImage()
        {
            SetButtonImageActivationStatus(false);
        }

        protected void SetButtonImageActivationStatus(bool status)
        {
            _buttonImage.gameObject.SetActive(status);
        }
    }
}