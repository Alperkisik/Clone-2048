using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] List<GameObject> Levels;

    public static Game_Manager instance;

    public event EventHandler OnLevelFailed;
    public event EventHandler OnLevelSuccess;
    public event EventHandler OnNextLevel;
    public event EventHandler OnRetryLevel;
    public event EventHandler OnLevelStarted;
    public event EventHandler OnLevelLoaded;

    [HideInInspector] public bool IslevelFinished;
    [HideInInspector] public bool IslevelStarted;

    int high_Score = 0;
    int levelIndex = 0;
    GameObject level;

    private void Awake()
    {
        instance = this;
        IslevelFinished = false;
    }

    private void Start()
    {
        high_Score = PlayerPrefs.GetInt("Highscore");
        InstantiateLevel(levelIndex);
    }

    public void LevelSuccess()
    {
        IslevelFinished = true;
        IslevelStarted = false;
        OnLevelSuccess?.Invoke(this, EventArgs.Empty);
    }

    public void LevelFailed()
    {
        IslevelFinished = true;
        IslevelStarted = false;
        OnLevelFailed?.Invoke(this, EventArgs.Empty);
    }

    public void StartLevel()
    {
        IslevelStarted = true;
        OnLevelStarted?.Invoke(this, EventArgs.Empty);
    }

    private void DestroyCurrentLevel()
    {
        Destroy(level);
    }

    private void InstantiateLevel(int levelIndex)
    {
        IslevelFinished = false;
        IslevelStarted = false;
        level = Instantiate(Levels[levelIndex], Vector3.zero, Quaternion.identity);
        OnLevelLoaded?.Invoke(this, EventArgs.Empty);
        StartLevel();
    }

    public void NextLevel()
    {
        levelIndex++;
        if (levelIndex >= Levels.Count) levelIndex = 0;
        DestroyCurrentLevel();
        InstantiateLevel(levelIndex);
    }

    public void RetryLevel()
    {
        DestroyCurrentLevel();
        InstantiateLevel(levelIndex);
    }

    public int Get_HighScore()
    {
        return high_Score;
    }

    public void Set_HighScore(int score)
    {
        high_Score = score;
        PlayerPrefs.SetInt("Highscore", high_Score);
        PlayerPrefs.Save();
    }
}
