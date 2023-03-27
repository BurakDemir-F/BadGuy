using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelSpecific.MobBakery
{
    public class FoodMachineTimeUI : MonoBehaviour
    {
        [SerializeField] private Image _timerImage;

        private void Start()
        {
            ResetUI();
        }

        public void ResetUI()
        {
            UpdateUI(0f);
        }

        public void UpdateUI(float ratio)
        {
            _timerImage.fillAmount = ratio;
        }

        public void ActivateUI()
        {
            _timerImage.gameObject.SetActive(true);
        }

        public void DeactivateUI()
        {
            _timerImage.gameObject.SetActive(false);
        }
    }
}