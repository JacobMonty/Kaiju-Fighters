using Unity.Collections;
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
        //Debug.Log("TOuch");

        if (collision.gameObject.name == "Kaiju")
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Collider>().enabled = false;
            FindFirstObjectByType<AudioSource>().Play();

            if (tag == "Red House")
            {
                FindFirstObjectByType<HouseWreckerManager>().EndGame();
                Destroy(collision.gameObject);
            }
            else if (tag == "Yellow House")
            {
                FindFirstObjectByType<HouseWreckerManager>().AddPoints(5);
            }
            else
            {
                FindFirstObjectByType<HouseWreckerManager>().AddPoints(1);
            }
            Destroy(gameObject, 3);
        }
    }
}
