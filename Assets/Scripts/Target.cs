// Target.cs
using UnityEngine;

public class Target : MonoBehaviour
{
    // Set this key in the Inspector for each target
    public KeyCode keyToPress; 

    private GameObject noteInTarget; // The note currently in our trigger

    void Update()
    {
        // Check if the correct key is pressed
        if (Input.GetKeyDown(keyToPress))
        {
            // Check if a note is currently in our trigger
            if (noteInTarget != null)
            {
                // We hit the note!
                Debug.Log("Hit!");
                Destroy(noteInTarget); // Destroy the note
                noteInTarget = null; // Clear the reference
                
                // Add score logic here
            }
            else
            {
                // We missed!
                Debug.Log("Miss!");
                // Add miss/penalty logic here
            }
        }
    }

    // This function is called when another collider ENTERS our trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is a Note
        if (other.CompareTag("Note"))
        {
            noteInTarget = other.gameObject;
        }
    }

    // This function is called when another collider LEAVES our trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the note that left is the one we were tracking, clear it
        if (other.gameObject == noteInTarget)
        {
            noteInTarget = null;
        }
    }
}