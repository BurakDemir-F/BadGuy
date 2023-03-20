using System;
using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Generic.Items
{
    public class ItemIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        private Vector3 _indicatorDefaultPos;
        private Tween _indicatorAnimTween;

        private void Start()
        {
            _indicatorDefaultPos = _indicator.transform.position;
            SetIndicatorStatus(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EnableIndicator();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {   
                DisableIndicator();
            }
        }

        private void EnableIndicator()
        {
            SetIndicatorStatus(true);
            PlayIndicatorAnim();
        }
        
        public void DisableIndicator()
        {
            SetIndicatorStatus(false);
            StopIndicatorAnim();
        }

        private void SetIndicatorStatus(bool status)
        {
            _indicator.SetActive(status);
        }

        private void PlayIndicatorAnim()
        {
            var topPos = _indicatorDefaultPos.SetY(_indicatorDefaultPos.y + .2f);
            _indicatorAnimTween = _indicator.transform.DOMove(topPos, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void StopIndicatorAnim()
        {
            _indicatorAnimTween?.Kill();
            _indicator.transform.position = _indicatorDefaultPos;
        }
    }
}