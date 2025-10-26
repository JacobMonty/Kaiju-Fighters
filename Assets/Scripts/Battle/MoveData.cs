using UnityEngine;

// Updated the menu name
[CreateAssetMenu(fileName = "New Move", menuName = "Kaiju/Create New Move")]
public class MoveData : ScriptableObject
{
    public string moveName;
    public int damage;
    public float cooldown;
    public string animationTrigger;
    public ParticleSystem particleEffect;
    public float damageDelay;
}
