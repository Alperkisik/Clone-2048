using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level_UI_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh_ScoreText;
    [SerializeField] TextMeshProUGUI textMesh_HighScore;

    int score = 0;

    void Start()
    {
        GetHighScore();
        Event_Listener();
    }

    private void Event_Listener()
    {
        Level_Manager.instance.OnPlayerGetScore += Event_LevelManager_OnPlayerGetScore;
        Level_Manager.instance.OnPlayerGetHighScore += Event_LevelManager_OnPlayerGetHighScore;
    }

    private void Event_LevelManager_OnPlayerGetHighScore(object sender, System.EventArgs e)
    {
        GetHighScore();
    }

    private void Event_LevelManager_OnPlayerGetScore(object sender, System.EventArgs e)
    {
        score = Level_Manager.instance.GetPlayerScore();
        textMesh_ScoreText.text = score.ToString();
    }

    private void GetHighScore()
    {
        textMesh_HighScore.text = Game_Manager.instance.Get_HighScore().ToString();
    }

    public void ReturnToMainMenu()
    {

    }
}
