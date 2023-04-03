using System;
using Managers;
using Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIManager _uiManager;
    private const string SavePrefName = "Level";

    public event Action LevelWin;
    public event Action GameFinished;
    public event Action LevelLoose;

    private int _level
    {
        get => PlayerPrefs.GetInt(SavePrefName, 0);
        set => PlayerPrefs.SetInt(SavePrefName, value);
    }

    public int Level => _level;

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
        var level = _level;
        if (level == 0)
        {
            _level++;
            LevelWin?.Invoke();
            return;
        }

        var isSceneMobBakery = SceneManager.GetActiveScene().name.Contains("mob", StringComparison.OrdinalIgnoreCase);
        
        if (isSceneMobBakery)
        {
            _level = 0;
            GameFinished?.Invoke();
        }
        else
            LevelWin?.Invoke();
    }

    public void GameLoose()
    {
        LevelLoose?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            GameWin();
        
        if (Input.GetKeyDown(KeyCode.Y))
            GameLoose();

    }
}