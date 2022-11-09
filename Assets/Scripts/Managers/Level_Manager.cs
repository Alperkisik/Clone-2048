using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    public static Level_Manager instance;

    public event EventHandler OnLevelEnded;
    public event EventHandler OnPlayerGetScore;
    public event EventHandler OnPlayerGetHighScore;

    int playerScore = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Event_Listener();
    }

    void Update()
    {

    }

    private void Event_Listener()
    {

    }

    private void LevelSuccess()
    {
        Game_Manager.instance.LevelSuccess();
    }

    private void LevelFailed()
    {
        Game_Manager.instance.LevelFailed();
    }

    public void Trigger_Event_LevelEnd()
    {
        OnLevelEnded?.Invoke(this, EventArgs.Empty);
    }

    public void IncreasePlayerScore(int value)
    {
        playerScore += value;

        if (Game_Manager.instance.Get_HighScore() < playerScore) 
        {
            Game_Manager.instance.Set_HighScore(playerScore);
        } 

        OnPlayerGetScore?.Invoke(this, EventArgs.Empty);
        OnPlayerGetHighScore?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }
}
