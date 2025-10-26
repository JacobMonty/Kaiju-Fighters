// Dropper.cs
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public KeyCode keyToPress;
    public IngredientType ingredientType;
    public GameObject ingredientPrefab; // Drag Cake, Frosting, etc. prefabs here

    private Plate currentPlate; // The plate currently under this dropper

    void Update()
    {
        // When the player presses the key AND a plate is underneath
        if (Input.GetKeyDown(keyToPress) && currentPlate != null)
        {
            //currentPlate.AddIngredient(ingredientType, ingredientPrefab);
        }
    }

    // When a plate ENTERS the dropper's trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            currentPlate = other.GetComponent<Plate>();
        }
    }

    // When a plate LEAVES the dropper's trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            currentPlate = null;
        }
    }
}