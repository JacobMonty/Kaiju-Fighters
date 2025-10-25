using System.Xml;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HouseWreckerManager : MonoBehaviour
{
    public GameObject house;
    public Sprite[] houseTypes;
    public Text scoreText;

    public Vector3 minPosition, maxPosition;

    public static int score = 0;

    private float timePassed = 0;
    private float houseSpawnInterval = 5;
    private int gameProgression = 1;
    private int houseSpawnNum = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > houseSpawnInterval)
        {
            spawnHouse();
            timePassed = 0f;
            gameProgression++; 
            if(houseSpawnInterval > 3) houseSpawnInterval -= gameProgression / 100f;
            if (gameProgression % 8 == 0 && houseSpawnNum < 16) houseSpawnNum += 2;
        }
    }

    private void spawnHouse()
    {

        for (int i = 0; i < houseSpawnNum; i++)
        {
            Vector3 randomPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
            );
            float randFloat = Random.Range(0, 100);
            GameObject newHouse = Instantiate(house, randomPosition, Quaternion.identity);

            if (randFloat < 5 + houseSpawnNum / 2)
            {
                newHouse.GetComponentAtIndex(0).GetComponent<SpriteRenderer>().sprite = houseTypes[2];
                newHouse.tag = "Yellow House";
            }
            else if (randFloat < 20 + gameProgression / 2)
            {
                newHouse.GetComponentAtIndex(0).GetComponent<SpriteRenderer>().sprite = houseTypes[0];
                newHouse.tag = "Red House";
            }
            else 
            { 
                newHouse.GetComponent<SpriteRenderer>().sprite = houseTypes[1];
                newHouse.tag = "Green House";
            }

            //Debug.Log(randFloat);
        }
        
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }
}
