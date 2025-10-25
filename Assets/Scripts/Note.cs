using UnityEngine;

public class Note : MonoBehaviour
{
    public float scrollSpeed = 5f;

    void Update()
    {
        // Move the note down based on the scroll speed and time
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Optional: Destroy the note if it goes off-screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}