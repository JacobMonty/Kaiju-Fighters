using UnityEngine;
using System.Collections.Generic; // To use Lists

public class Plate : MonoBehaviour
{
    // This list will store the ingredients we add
    public List<IngredientType> ingredients = new List<IngredientType>();
    
    private CookingGameManager gameManager;
    private float speed;
    private float yOffset = 0.5f; // To stack ingredients
    
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindFirstObjectByType<CookingGameManager>();
    }

    void Update()
    {
        // Get the current speed from the manager
        speed = gameManager.GetCurrentSpeed();
        
        // Move the plate along the conveyor
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    // This function will be called by the Droppers
    public void AddIngredient(IngredientType type, GameObject ingredientPrefab)
    {
        ingredients.Add(type);
        
        // --- Visual Part ---
        // Spawn the ingredient sprite (e.g., the frosting)
        Vector3 spawnPos = transform.position + new Vector3(0, yOffset * ingredients.Count, 0);
        GameObject ingredientVisual = Instantiate(ingredientPrefab, spawnPos, Quaternion.identity);
        
        // Make the ingredient a child of the plate so it moves with it
        ingredientVisual.transform.SetParent(this.transform);
    }
}