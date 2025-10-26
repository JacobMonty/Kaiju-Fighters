using UnityEngine;

// Updated the menu name
[CreateAssetMenu(fileName = "New Move", menuName = "Kaiju/Create New Move")]
public class MoveData : ScriptableObject
{
    public string moveName;
    public int damage;
    public float cooldown; // This will now be the *shared* cooldown
    public string animationTrigger;
    public ParticleSystem particleEffect;
    public float damageDelay; // How long after animation starts to deal damage
}
