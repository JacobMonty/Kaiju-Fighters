using UnityEditor.SearchService;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public float power = 10f;
    public float defense = 10f;
    public float speed = 5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
}
