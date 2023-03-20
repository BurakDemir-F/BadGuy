using System;
using System.Collections;
using DG.Tweening;
using Generic.Interaction;
using Generic.Items.SO;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Generic.Items
{
    public class PickableItem : InteractionListener
    {
        [SerializeField] private CD_PickableItem _itemData;
        [SerializeField] private FlyObjectBehaviour _flyObject;
        [SerializeField] private Image _percentageIndicator;
        private Coroutine _fillCor;
        private YieldInstruction _wait;
        private bool _isItemPicked;

        public override string Tag => tag;
        public override Type RequestedComponentType => typeof(Transform);
        public override ComponentProvider ComponentProvider => ComponentProvider.Trigger;
        public event Action ItemPicked;

        private void Start()
        {
            SetIndicatorStatus(false);
            _wait = new WaitForEndOfFrame();
        }

        public override void OnTriggerEntered(Object obj)
        {
            _flyObject.StopIndicatorAnim();
            if(_fillCor != null)
                StopCoroutine(_fillCor);
            _fillCor = StartCoroutine(IndicatorFillCor(obj as Transform));
            SetFillAmount(0f);
            SetIndicatorStatus(true);
        }

        public override void OnTriggerExited(Object obj)
        {
            if (!_isItemPicked)
                _flyObject.PlayIndicatorAnim();
            
            SetFillAmount(0f);
            SetIndicatorStatus(false);
        }

        private IEnumerator IndicatorFillCor(Transform target)
        {
            var counter = 0f;
            var pickTime = _itemData.PickTime;
            while (counter < pickTime)
            {
                SetFillAmount(counter/pickTime);
                counter += Time.deltaTime;
                yield return _wait;
            }

            SetIndicatorStatus(false);
            var seq = DOTween.Sequence();
            seq.Append(_flyObject.transform.DOMove(target.position, .2f))
                .Append(_flyObject.transform.DOScale(Vector3.zero,.2f))
                .OnComplete(() =>
                {
                    _isItemPicked = true;
                    ItemPicked?.Invoke();
                });
        }

        private void SetIndicatorStatus(bool status)
        {
            _percentageIndicator.gameObject.SetActive(status);
        }

        private void SetFillAmount(float amount)
        {
            amount = Mathf.Clamp01(amount);
            _percentageIndicator.fillAmount = amount;
        }
    }
}