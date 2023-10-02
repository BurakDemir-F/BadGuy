using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Patterns.Singleton<GameManager>
{
    [SerializeField] private UIManager _uiManager;
    private const string SavePrefName = "Level";

    public event Action LevelWin;
    public event Action GameFinished;
    public event Action LevelLoose;

    public int Level
    {
        get => PlayerPrefs.GetInt(SavePrefName, 0);
        set => PlayerPrefs.SetInt(SavePrefName, value);
    }
    

    private int _mainMenuIndex = 0;
    private int _missionPoisonIndex = 1;
    private int _mobBakeryIndex = 2;

    public void LoadMissionPoison()
    {
        SceneManager.LoadScene(_missionPoisonIndex);
    }

    public void LoadMobBakery()
    {
        SceneManager.LoadScene(_mobBakeryIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenuIndex);
    }

    public void GameWin()
    {
        if (Level == 0)
        {
            Level++;
            LevelWin?.Invoke();
            return;
        }

        var isSceneMobBakery = SceneManager.GetActiveScene().name.Contains("mob", StringComparison.OrdinalIgnoreCase);

        if (isSceneMobBakery)
        {
            Level = 0;
            GameFinished?.Invoke();
        }
        else
        {
            LevelWin?.Invoke();
        }
    }

    public void GameLoose()
    {
        LevelLoose?.Invoke();
    }

    private void OnAddCompleted()
    {
        Debug.Log("add completed");
    }

    private void AddFailCallback()
    {
        Debug.Log("add fail callback");
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            GameWin();

        if (Input.GetKeyDown(KeyCode.Y))
            GameLoose();
    }
#endif
}