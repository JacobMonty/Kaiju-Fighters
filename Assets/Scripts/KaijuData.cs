using System.Collections.Generic;
using UnityEngine;

// This replaces PokemonData.cs
[CreateAssetMenu(fileName = "New Kaiju", menuName = "Kaiju/Create New Kaiju")]
public class KaijuData : ScriptableObject
{
    public string kaijuName; // Used for the nametag
    public int maxHealth;
    public float dodgeChance;  // A value from 0 to 100
    
    // The list of moves this Kaiju can use
    public List<MoveData> moves = new List<MoveData>();
}
