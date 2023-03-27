using System;
using UnityEngine;
using UnityEngine.UI;

namespace Generic.ShowSystem
{
    public class ProductButton : InputButton
    {
        [SerializeField] private Image _passiveImage;

        private bool _isPassive;

        private void Start()
        {
            DeactivatePassiveImage();
        }

        public void SetPassive()
        {
            DeactivateButtonImage();
            ActivatePassiveImage();
            _isPassive = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            DeactivatePassiveImage();
        }

        public override void Click()
        {
            if(_isPassive)
                return;
            
            base.Click();
        }

        public override void Activate()
        {
            _isPassive = false;
            DeactivatePassiveImage();
            base.Activate();
        }

        protected void ActivatePassiveImage()
        {
            SetPassiveImageActivationStatus(true);
        }
        
        protected void DeactivatePassiveImage()
        {
            SetPassiveImageActivationStatus(false);
        }
        
        protected void SetPassiveImageActivationStatus(bool status)
        {
            _passiveImage.gameObject.SetActive(status);
        }
    }
}