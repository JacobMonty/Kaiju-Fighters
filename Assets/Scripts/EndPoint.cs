using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public CookingGameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When a plate hits the end of the line
        if (other.CompareTag("Plate"))
        {
            Plate plate = other.GetComponent<Plate>();
            
            // Tell the manager to check the plate
            gameManager.CheckPlate(plate);
            
            // Destroy the finished plate
            Destroy(other.gameObject);
        }
    }
}