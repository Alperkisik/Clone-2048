using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : MonoBehaviour
{
    public int score;
    public int[,] board_values;
    public int boardWidth;

    public LevelData(Level level)
    {
        score = level.score;
        board_values = level.board_values;
        boardWidth = level.boardWidth;
    }
}
