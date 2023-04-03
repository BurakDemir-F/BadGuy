using System;
using System.Collections;
using DG.Tweening;
using Patterns;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private TextMeshProUGUI _gameFinishedText;
        public Button playButton;
        public Button musicOnOffButton;

        private IEnumerator Start()
        {
            var level = _gameManager.Level;
            playButton.onClick.AddListener(LoadScene);
            _gameManager.LevelWin += OnLevelWin;
            _gameManager.GameFinished += OnGameFinished;
            _gameManager.LevelLoose += OnLevelLoose;

            yield return new WaitForSeconds(2f);
            FadeButton(true);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var isSceneMainMenu = arg0.name.Contains("main", StringComparison.OrdinalIgnoreCase);
            if(isSceneMainMenu)
                FadeButton(true);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(LoadScene);
            _gameManager.LevelWin -= OnLevelWin;
            _gameManager.GameFinished -= OnGameFinished;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _gameManager.LevelLoose -= OnLevelLoose;
        }

        private void OnLevelLoose()
        {
            LoadLevel();
        }

        private void FadeButton(bool isFadeIn)
        {
            if(isFadeIn)
                playButton.gameObject.SetActive(true);
            var buttonColor = playButton.image.color;
            var fullColor = buttonColor.GetFullAlpha();
            var zeroColor = buttonColor.GetZeroAlpha();

            playButton.image.color = isFadeIn ? zeroColor : fullColor;
            var targetColor = isFadeIn ? fullColor : zeroColor;
            playButton.image.DOColor(targetColor, 2f).OnComplete(() =>
            {
                if (!isFadeIn)
                {
                    playButton.gameObject.SetActive(false);
                }
            });
        }

        [ContextMenu("Load Scene Cor")]
        public void LoadScene()
        {
            FadeButton(false);
            LoadLevel();
        }

        private void LoadLevel()
        {
            Debug.Log("load level called.");
            var level = _gameManager.Level;
            StartCoroutine(LoadSceneCor(level));
        }

        private IEnumerator LoadSceneCor(int level)
        {
            if (level == 0)
            {
                yield return StartCoroutine(_loadingScreen.FadeInCor(LoadingType.MissionPoison));
                _gameManager.LoadMissionPoison();
                StartCoroutine(_loadingScreen.FadeOutCor());
            }
            else
            {
                yield return StartCoroutine(_loadingScreen.FadeInCor(LoadingType.MobBakery));
                _gameManager.LoadMobBakery();
                StartCoroutine(_loadingScreen.FadeOutCor());
            }
        }

        private void OnLevelWin()
        {
            LoadLevel();
        }

        private void OnGameFinished()
        {
            StartCoroutine(GameFinishedCor());
        }

        private IEnumerator GameFinishedCor()
        {
            yield return StartCoroutine(_loadingScreen.FadeInCor(LoadingType.MissionPoison));
            _gameFinishedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            _gameManager.LoadMainMenu();
            StartCoroutine(_loadingScreen.FadeOutCor());
        }
    }
}