using UnityEngine;
using System.Collections;
using TMPro; // For UI text
using UnityEngine.SceneManagement; // To restart the game

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float initialScrollSpeed = 4f;
    public float speedIncreasePerHit = 0.1f;
    public float initialSpawnDelay = 1.5f;
    
    [HideInInspector] // This will be set by the Note script
    public float currentScrollSpeed;
    private int score = 0;
    public bool isGameOver = false;

    [Header("Object Links")]
    public GameObject noteLeftPrefab;
    public GameObject noteRightPrefab;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public Animator characterAnimator; // We'll use this in Part 4

    [Header("UI Links")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // Reset everything on start
        currentScrollSpeed = initialScrollSpeed;
        isGameOver = false;
        Time.timeScale = 1; // Make sure the game is not paused
        gameOverPanel.SetActive(false);
        scoreText.text = "Score: 0";
        
        // Start the endless spawning
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        // This loop will run forever until the game is over
        while (!isGameOver)
        {
            // Wait for a random time before spawning the next note
            // You can make this delay get shorter over time, too!
            float spawnDelay = Random.Range(initialSpawnDelay * 0.8f, initialSpawnDelay * 1.2f);
            yield return new WaitForSeconds(spawnDelay);

            // Randomly pick left (0) or right (1)
            int noteType = Random.Range(0, 2);

            if (noteType == 0)
            {
                Instantiate(noteLeftPrefab, spawnPointLeft.position, Quaternion.identity);
            }
            else
            {
                Instantiate(noteRightPrefab, spawnPointRight.position, Quaternion.identity);
            }
        }
    }

    public void AddScore()
    {
        if (isGameOver) return;

        score++;
        scoreText.text = "Score: " + score;
        
        // Speed up the game
        currentScrollSpeed += speedIncreasePerHit;
    }

    public void HandleMiss()
    {
        if (isGameOver) return; // Only trigger game over once
        
        isGameOver = true;
        Time.timeScale = 0; // Pause the game

        // Show the game over screen
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        
        // You would also spawn your "Miss" animation here
    }

    // Create a public function for your Restart Button
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}