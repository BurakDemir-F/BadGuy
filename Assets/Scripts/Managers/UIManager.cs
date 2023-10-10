using System;
using System.Collections;
using CrazyGames;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class UIManager : Patterns.Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private TextMeshProUGUI _gameFinishedText;
        [SerializeField] private GameObject _gameFinishedRoot;
        public Button newGameButton;
        [SerializeField] private TMP_Text _newGameText;
        public Button continueButton;
        [SerializeField] private TMP_Text _continueGameText;

        private IEnumerator Start()
        {
            newGameButton.onClick.AddListener(PlayNew);
            _gameManager.LevelWin += OnLevelWin;
            _gameManager.GameFinished += OnGameFinished;
            _gameManager.LevelLoose += OnLevelLoose;
            continueButton.onClick.AddListener(LoadScene);

            yield return new WaitForSeconds(2f);
           
            FadeUI();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void FadeUI()
        {
            var level = _gameManager.Level;
            FadeButton(newGameButton, true);
            FadeText(_newGameText, true);
            if (level > 0)
            {
                continueButton.gameObject.SetActive(true);
                FadeButton(continueButton, true);
                FadeText(_continueGameText, true);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var isSceneMainMenu = arg0.name.Contains("main", StringComparison.OrdinalIgnoreCase);
            if (isSceneMainMenu)
            {
                FadeUI();

                Cursor.lockState = CursorLockMode.None;
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

        private void FadeButton(Button button, bool isFadeIn, Action callback = null)
        {
            if (isFadeIn)
                button.gameObject.SetActive(true);

            StartCoroutine(FadeUICor((color) => button.image.color = color,
                () => button.image.color,
                isFadeIn,
                () =>
                {
                    if (!isFadeIn)
                    {
                        button.gameObject.SetActive(false);
                    }

                    callback?.Invoke();
                }
            ));
        }

        private void FadeText(TMP_Text text, bool isFadeIn, Action endCallback = null)
        {
            if (isFadeIn)
                text.gameObject.SetActive(true);

            StartCoroutine(FadeUICor((color) => text.color = color,
                () => text.color,
                isFadeIn,
                () =>
                {
                    if (!isFadeIn)
                    {
                        text.gameObject.SetActive(false);
                    }

                    endCallback?.Invoke();
                }
            ));
        }

        private IEnumerator FadeUICor(Action<Color> changeFadeValue, Func<Color> getColorValue, bool isFadeIn,
            Action endCallback)
        {
            var buttonColor = getColorValue.Invoke();
            var fullColor = buttonColor.GetFullAlpha();
            var zeroColor = buttonColor.GetZeroAlpha();

            var startColor = isFadeIn ? zeroColor : fullColor;
            var targetColor = isFadeIn ? fullColor : zeroColor;
            changeFadeValue.Invoke(isFadeIn ? zeroColor : fullColor);

            var elapsedTime = 0f;
            var duration = 2f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var value = elapsedTime / duration;
                var color = Color.Lerp(startColor, targetColor, value);
                changeFadeValue.Invoke(color);
                yield return null;
            }

            endCallback?.Invoke();
        }

        private void PlayNew()
        {
            CrazyEvents.Instance.GameplayStart();
            _gameManager.Level = 0;
            LoadScene();
        }

        [ContextMenu("Load Scene Cor")]
        public void LoadScene()
        {
            Cursor.lockState = CursorLockMode.Locked;
            FadeButton(newGameButton, false);
            FadeText(_newGameText,false);
            FadeButton(continueButton, false);
            FadeText(_continueGameText,false,LoadLevel);
            //LoadLevel();
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