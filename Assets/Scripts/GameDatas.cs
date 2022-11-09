using UnityEngine;

[System.Serializable]
public class GameDatas : MonoBehaviour
{
    public int score;
    public int[,] gameBoard_Values;

    public GameDatas(GameData gameData)
    {
        score = gameData.score;
        gameBoard_Values = gameData.boardValues;
    }
}
