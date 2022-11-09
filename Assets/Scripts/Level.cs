using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int score;
    public int[,] board_values;
    public int boardWidth;
    public bool levelLoaded = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        levelLoaded = false;
    }

    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    }

    public void LoadLevel()
    {
        LevelData data = SaveSystem.LoadLevel();
        levelLoaded = true;
        score = data.score;
        board_values = data.board_values;
        boardWidth = data.boardWidth;
    }
}
