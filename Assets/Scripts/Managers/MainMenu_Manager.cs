using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh_HighScore;
    [SerializeField] Level level;
    [SerializeField] GameObject continueButton;
    int highScore = 0;

    private void Start()
    {
        SetHighScore();
        level.LoadLevel();

        if (level.score == 0) continueButton.SetActive(false); else continueButton.SetActive(true);
    }

    private void SetHighScore()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        textMesh_HighScore.text = highScore.ToString();
    }

    public void Continue()
    {
        GameMode.instance.new_game = false;
        SceneManager.LoadScene("Game Scene");
    }

    public void NewGame()
    {
        GameMode.instance.new_game = true;
        SceneManager.LoadScene("Game Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
