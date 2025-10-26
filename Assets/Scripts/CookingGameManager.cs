using UnityEngine;
using TMPro; // For the UI text
using System.Collections.Generic;
using UnityEngine.UI; // To use Lists

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
    public Sprite[] Sprites;
    public TextMeshProUGUI scoreText; // Add a score variable if you want
    
    // This will hold the "correct" order
    private List<int> currentOrder = new List<int>();

    void Start()
    {
        currentConveyorSpeed = baseConveyorSpeed;
        GenerateNewOrder();
        SpawnNewPlate();
    }

    void Update()
    {

        int randomNum = Random.Range(0, 10000);
        if(randomNum > 10000 - 90)
        {
            SpawnNewPlate();
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
        for(int i = 0; i < 2; i++) currentOrder.Add(Random.Range(0, 4));
        Debug.Log("New Order: " + GameObject.Find("Order1Image"));

        GameObject.Find("Order1Image").GetComponent<Image>().sprite = Sprites[currentOrder[0]];
        GameObject.Find("Order2Image").GetComponent<Image>().sprite = Sprites[currentOrder[1]];
    }

    void SpawnNewPlate()
    {
        Instantiate(platePrefab, plateSpawnPoint.position, Quaternion.identity);
    }

    // This is called by the EndPoint when a plate arrives
    public void CheckPlate(Plate plate)
    {

        // --- Compare the plate's list to the currentOrder list ---
        
        if(currentOrder[0] != -1 && plate.SushiType == currentOrder[0])
        {
            // delete first order
            GameObject.Find("Order1Image").GetComponent<Image>().sprite = null;
            currentOrder[0] = -1;
        } 
        else if (currentOrder[1] != -1 && plate.SushiType == currentOrder[1])
        {
            // delete second order
            GameObject.Find("Order1Image").GetComponent<Image>().sprite = Sprites[currentOrder[1]];
            currentOrder[1] = -1;
        }

        streak++;
        currentConveyorSpeed = baseConveyorSpeed + (streak * speedIncrease);
        // Add score
        
        if(currentOrder[0] == -1 && currentOrder[1] == -1) GenerateNewOrder();
    }

    void EndGame()
    {
        // Stop the game
        Time.timeScale = 0; // Pauses everything
        Debug.Log("Game Over!");
        // TODO: Show a "Results" screen or load the MainGame scene
    }
}