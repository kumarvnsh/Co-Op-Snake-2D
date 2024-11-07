using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text scoreText;         // Reference to the UI text that displays the score
    public GameObject gameOverPanel; // Reference to the game over panel
    private int score;              // Keeps track of the player's score
    public GameObject pauseMenu;




    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UIManager");
                    instance = obj.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        score = 0;
        UpdateScoreText();
        gameOverPanel.SetActive(false); // Hide the game over panel at the start
    }

    // Method to add to the score
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // Method to update the score text on the UI
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Method to trigger game over
    public void GameOver()
    {
        gameOverPanel.SetActive(true); // Show the game over panel
    }

    // Method to restart the current level
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
