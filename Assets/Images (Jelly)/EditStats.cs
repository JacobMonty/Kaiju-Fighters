using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditStats : MonoBehaviour
{
    [SerializeField] public TextMeshPro statsText;
    public void editStats(string newStats)
    {
        statsText.text = newStats;    
    }
}
