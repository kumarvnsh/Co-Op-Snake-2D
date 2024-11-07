using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoOpUIManager : MonoBehaviour
{
    public Text player1ScoreText;
    public Text player2ScoreText;
    public GameObject gameOverPanel;
    public Text GameOverText;
    private int player1Score = 0;
    private int player2Score = 0;
    public GameObject pauseMenu;

    public static CoOpUIManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddScore(int amount, int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score += amount;
            UpdateScoreText(player1ScoreText, player1Score);
        }
        else if (playerNumber == 2)
        {
            player2Score += amount;
            UpdateScoreText(player2ScoreText, player2Score);
        }
    }

    private void UpdateScoreText(Text scoreText, int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver(int playerNumber)
    {
        gameOverPanel.SetActive(true);
        GameOverText.text = "Player " + playerNumber + " Lost!";
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
