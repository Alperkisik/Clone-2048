using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int score;
    public int boardWidth;
    public bool levelLoaded = false;
    public string boardValues_text;

    private void Start()
    {
        levelLoaded = false;

        LoadLevel();
    }

    public void SaveLevel()
    {
        //Debug.Log(boardValues_text);
        SaveSystem.SaveLevel(this);
    }

    public void LoadLevel()
    {
        LevelData data = SaveSystem.LoadLevel();
        if(data != null)
        {
            if (score > 0) 
            {
                levelLoaded = true;
                score = data.score;
                boardWidth = data.boardWidth;
                boardValues_text = data.boardValues_text;
            } 
            else levelLoaded = false;
        }
        else levelLoaded = false;
    }
}
