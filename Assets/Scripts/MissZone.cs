using UnityEngine;

public class MissZone : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        // This is a "parent" tag for all notes
        gameObject.tag = "Note";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If a note hits this, it's a miss
        if (other.CompareTag("NoteLeft") || other.CompareTag("NoteRight"))
        {
            gameManager.HandleMiss();
            Destroy(other.gameObject); // Destroy the missed note
        }
    }
}