using UnityEngine;

public class Note : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // Move the note down based on the GameManager's current speed
        if (gameManager != null && !gameManager.isGameOver)
        {
            transform.Translate(Vector3.down * gameManager.currentScrollSpeed * Time.deltaTime);
        }
    }
}