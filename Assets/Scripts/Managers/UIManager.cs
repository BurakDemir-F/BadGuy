using System;
using System.Collections;
using DG.Tweening;
using Patterns;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private TextMeshProUGUI _gameFinishedText;
        [SerializeField] private GameObject _gameFinishedRoot;
        public Button newGameButton;
        public Button continueButton;

        private IEnumerator Start()
        {
            var level = _gameManager.Level;
            newGameButton.onClick.AddListener(PlayNew);
            _gameManager.LevelWin += OnLevelWin;
            _gameManager.GameFinished += OnGameFinished;
            _gameManager.LevelLoose += OnLevelLoose;
            continueButton.gameObject.SetActive(level>0);
            continueButton.onClick.AddListener(LoadScene);

            yield return new WaitForSeconds(2f);
            FadeButton(newGameButton,true);
            if(level>0)
                FadeButton(continueButton,true);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var isSceneMainMenu = arg0.name.Contains("main", StringComparison.OrdinalIgnoreCase);
            if(isSceneMainMenu)
            {
                FadeButton(newGameButton,true);
                FadeButton(continueButton,true);
            }
        }

        private void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(PlayNew);
            continueButton.onClick.RemoveListener(LoadScene);
            _gameManager.LevelWin -= OnLevelWin;
            _gameManager.GameFinished -= OnGameFinished;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _gameManager.LevelLoose -= OnLevelLoose;
        }

        private void OnLevelLoose()
        {
            LoadLevel();
        }

        private void FadeButton(Button button, bool isFadeIn)
        {
            if(isFadeIn)
                button.gameObject.SetActive(true);
            var buttonColor = button.image.color;
            var fullColor = buttonColor.GetFullAlpha();
            var zeroColor = buttonColor.GetZeroAlpha();

            button.image.color = isFadeIn ? zeroColor : fullColor;
            var targetColor = isFadeIn ? fullColor : zeroColor;
            button.image.DOColor(targetColor, 2f).OnComplete(() =>
            {
                if (!isFadeIn)
                {
                    button.gameObject.SetActive(false);
                }
            });
        }

        private void PlayNew()
        {
            _gameManager.Level = 0;
            LoadScene();
        }
        
        [ContextMenu("Load Scene Cor")]
        public void LoadScene()
        {
            FadeButton(newGameButton,false);
            FadeButton(continueButton,false);
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
            _gameFinishedRoot.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            _gameFinishedText.gameObject.SetActive(false);
            _gameFinishedRoot.gameObject.SetActive(false);
            _gameManager.LoadMainMenu();
            StartCoroutine(_loadingScreen.FadeOutCor());
        }
    }
}