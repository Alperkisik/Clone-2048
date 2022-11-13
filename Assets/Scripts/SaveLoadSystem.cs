using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public int board_width;
    //board[,] values saves as a single word into a string. Must be converted to get each x,y value
    //0 index x:1,y:1 , 1 index x:1,y:2, 2 index x:1,y3 and this goes on
    //a 0 b 2 c 4 d 8 e 16 f 32 g 64 h 128 i 256 j 512 k 1024 
    public string board_values_text; 
    public int score;
    public int highScore;

    private void Awake()
    {
        LoadLevel();
    }

    public void SaveLevel()
    {
        PlayerPrefs.SetInt("Board Width", board_width);

        PlayerPrefs.SetInt("Score", score);

        PlayerPrefs.SetString("Board Values Text", board_values_text);

        PlayerPrefs.Save();

        Debug.Log("Level Saved.");
    }

    public void LoadLevel()
    {
        score = PlayerPrefs.GetInt("Score");
        board_values_text = PlayerPrefs.GetString("Board Values Text");
        board_width = PlayerPrefs.GetInt("Board Width");
        highScore = PlayerPrefs.GetInt("Highscore");
    }

    public void ResetData()
    {
        score = 0;
        board_values_text = "";
        board_width = 0;

        SaveLevel();
    }
}
