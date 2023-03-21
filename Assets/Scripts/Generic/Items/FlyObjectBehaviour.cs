using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Generic.Items
{
    public class FlyObjectBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _flyObject;
        [SerializeField] private bool _hideOnStart;
        private Vector3 _defaultPos;
        private Tween _animTween;

        private void Awake()
        {
            _defaultPos = _flyObject.transform.position;
            SetIndicatorStatus(!_hideOnStart);
            if(!_hideOnStart)
                PlayIndicatorAnim();
        }
    
        public void EnableIndicator()
        {
            SetIndicatorStatus(true);
            PlayIndicatorAnim();
        }

        public void DisableIndicator()
        {
            SetIndicatorStatus(false);
            StopIndicatorAnim();
        }

        public void SetIndicatorStatus(bool status)
        {
            _flyObject.SetActive(status);
        }

        public void PlayIndicatorAnim()
        {
            var topPos = _defaultPos.SetY(_defaultPos.y + .2f);
            _animTween = _flyObject.transform.DOMove(topPos, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopIndicatorAnim()
        {
            _animTween?.Kill();
            _flyObject.transform.position = _defaultPos;
        }
    }
}