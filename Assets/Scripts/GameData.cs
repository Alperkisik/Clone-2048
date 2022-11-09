using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int score;
    public int[,] boardValues;
    public bool continueProgress = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SaveGameData()
    {
        SaveSystem.SaveGame(this);
    }
}
