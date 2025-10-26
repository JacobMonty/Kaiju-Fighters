using UnityEngine;
using TMPro; // For the UI text
using System.Collections.Generic; // To use Lists

public class CookingGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameTimer = 60f;
    public float baseConveyorSpeed = 2f;
    public float speedIncrease = 0.5f;
    private float currentConveyorSpeed;
    private int streak = 0;

    [Header("Object Links")]
    public GameObject platePrefab;
    public Transform plateSpawnPoint;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText; // Add a score variable if you want
    
    // This will hold the "correct" order
    private List<IngredientType> currentOrder = new List<IngredientType>();

    void Start()
    {
        currentConveyorSpeed = baseConveyorSpeed;
        GenerateNewOrder();
        SpawnNewPlate();
    }

    void Update()
    {
        // --- Timer Logic ---
        gameTimer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(gameTimer).ToString();

        if (gameTimer <= 0)
        {
            EndGame();
        }
    }

    public float GetCurrentSpeed()
    {
        return currentConveyorSpeed;
    }

    void GenerateNewOrder()
    {
        // This is a simple example. You can make this more complex.
        currentOrder.Clear();
        currentOrder.Add(IngredientType.Cake);
        currentOrder.Add(IngredientType.Frosting);
        
        // TODO: Update the UI (OrderDisplay) to show these ingredients
        Debug.Log("New Order: Cake and Frosting");
    }

    void SpawnNewPlate()
    {
        Instantiate(platePrefab, plateSpawnPoint.position, Quaternion.identity);
    }

    // This is called by the EndPoint when a plate arrives
    public void CheckPlate(Plate plate)
    {
        // --- Compare the plate's list to the currentOrder list ---
        bool isCorrect = true;
        if (plate.ingredients.Count != currentOrder.Count)
        {
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < currentOrder.Count; i++)
            {
                if (plate.ingredients[i] != currentOrder[i])
                {
                    isCorrect = false;
                    break;
                }
            }
        }

        // --- Handle Correct or Incorrect ---
        if (isCorrect)
        {
            Debug.Log("Correct Order!");
            streak++;
            currentConveyorSpeed = baseConveyorSpeed + (streak * speedIncrease);
            // Add score
        }
        else
        {
            Debug.Log("Wrong Order!");
            streak = 0;
            currentConveyorSpeed = baseConveyorSpeed; // Reset speed
        }
        
        GenerateNewOrder();
        SpawnNewPlate();
    }

    void EndGame()
    {
        // Stop the game
        Time.timeScale = 0; // Pauses everything
        Debug.Log("Game Over!");
        // TODO: Show a "Results" screen or load the MainGame scene
    }
}