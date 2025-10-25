using UnityEngine;

public class HouseDestroy : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 20); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TOuch");
        if (collision.gameObject.name == "Kaiju")
        {
            FindFirstObjectByType<HouseWreckerManager>().AddPoints(1);
            Destroy(gameObject);
        }
    }
}
