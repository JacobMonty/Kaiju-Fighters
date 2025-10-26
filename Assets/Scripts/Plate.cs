using UnityEngine;
using System.Collections.Generic; // To use Lists
using Unity.VisualScripting;

public class Plate : MonoBehaviour
{
    // This list will store the ingredients we add
    public Sprite[] Sprites;
    
    public int SushiType;
    private CookingGameManager gameManager;
    private float speed;
    
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindFirstObjectByType<CookingGameManager>();

        SushiType = Random.Range(0,4);
        GetComponent<SpriteRenderer>().sprite = Sprites[SushiType];
    }

    void Update()
    {
        // Get the current speed from the manager
        speed = gameManager.GetCurrentSpeed();
        
        // Move the plate along the conveyor
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}