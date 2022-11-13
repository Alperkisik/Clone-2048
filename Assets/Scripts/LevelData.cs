using System;

[Serializable]
public class LevelData
{
    public int score;
    public string boardValues_text;
    public int boardWidth;

    public LevelData(Level level)
    {
        score = level.score;
        boardValues_text = level.boardValues_text;
        boardWidth = level.boardWidth;
    }
}
