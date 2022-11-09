using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh_HighScore;
    int highScore = 0;

    private void Start()
    {
        SetHighScore();
    }

    private void SetHighScore()
    {
        textMesh_HighScore.text = highScore.ToString();
    }

    public void Continue()
    {
        GameDatas data = SaveSystem.LoadGameDatas();
        if(data != null) SceneManager.LoadScene("Game Scene");
        SceneManager.LoadScene("Game Scene");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
