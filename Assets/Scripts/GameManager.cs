// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Drag your Note prefab from the Project folder here
    public GameObject notePrefab; 

    // Drag your spawn point objects from the Hierarchy here
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    // This is just for testing
    void Update()
    {
        // Press '1' to spawn a note on the left
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(notePrefab, spawnPointLeft.position, Quaternion.identity);
        }

        // Press '2' to spawn a note on the right
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(notePrefab, spawnPointRight.position, Quaternion.identity);
        }
    }
}