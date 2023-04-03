using System.Collections;
using DG.Tweening;
using Managers.SO;
using Patterns;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class LoadingScreen : Singleton<LoadingScreen>
    {
        [SerializeField] private CD_LoadingScreen _screenData;
        [SerializeField] private Image _loadingImage;

        [SerializeField] private GameObject _missionPoisonObj;
        [SerializeField] private GameObject _mobBakeryObj;

        private Tween _fadeTween;
        private Tween _rotationTween;
        private LoadingScreenData _currentData;

        private void Start()
        {
            _loadingImage.gameObject.SetActive(false);
            CreateLoadingObjects();
        }

        private void CreateLoadingObjects()
        {
            var poisonData = _screenData.MissionPoisonLoading;
            var bakeryData = _screenData.MobBakeryLoading;

            CreateObjSetData(_missionPoisonObj, poisonData);
            CreateObjSetData(_mobBakeryObj, bakeryData);

            void CreateObjSetData(GameObject obj, LoadingScreenData data)
            {
                data.LoadingObject = obj;
                obj.gameObject.SetActive(false);
            }
        }

        public IEnumerator FadeInCor(LoadingType type)
        {
            LoadingScreenData data = null;

            switch (type)
            {
                case LoadingType.MissionPoison:
                    data = _screenData.MissionPoisonLoading;
                    break;
                case LoadingType.MobBakery:
                    data = _screenData.MobBakeryLoading;
                    break;
            }

            _currentData = data;
            ChangeActivationStatus(true);
            return FadeCor(true);
        }

        public IEnumerator FadeOutCor()
        {
            yield return FadeCor(false);
            ChangeActivationStatus(false);
        }

        private IEnumerator FadeCor(bool isFadeIn)
        {
            var color = _loadingImage.color;
            var startColor = isFadeIn ? color.GetZeroAlpha() : color.GetFullAlpha();
            var targetColor = isFadeIn ? color.GetFullAlpha() : color.GetZeroAlpha();

            _loadingImage.color = startColor;
            _fadeTween = _loadingImage.DOColor(targetColor, _screenData.FadeDuration);
            yield return _fadeTween.WaitForCompletion();
        }

        private void ChangeActivationStatus(bool status)
        {
            if (status)
                _loadingImage.sprite = _currentData?.LoadingSprite;
            _loadingImage.gameObject.SetActive(status);
        }
    }

    public enum LoadingType
    {
        None,
        MissionPoison,
        MobBakery
    }
}