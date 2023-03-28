using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LevelSpecific.MobBakery
{
    public class TimerUI : MonoBehaviour
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

        public Coroutine StartTimer(float duration, Action endCallback)
        {
            return StartCoroutine(TimerCor(duration, endCallback));
        }
        
        protected IEnumerator TimerCor(float duration, Action endCallback)
        {
            ActivateUI();
            ResetUI();
            var timer = 0f;
            while (timer < duration)
            {
                UpdateUI(timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            UpdateUI(1f);
            DeactivateUI();
            endCallback?.Invoke();
        }
    }
}