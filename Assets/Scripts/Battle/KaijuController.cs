using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Needed for Coroutines

// This replaces PokemonController.cs
public class KaijuController : MonoBehaviour
{
    [Header("Data")]
    public KaijuData kaijuData; // We will assign this at the start
    
    [Header("Live Stats")]
    private int currentHealth;
    public bool isDodging = false;
    // public bool isDefending = false; // Removed Defend
    public bool isStunned = false; // Prevents acting
    
    [Header("Components")]
    public Slider healthSlider;
    public GameObject powerUpGlow; // The glow effect

    private FightManager fightManager; // Reference to the referee
    
    // NEW: For Dodge movement
    private Vector3 originPosition;
    public float dodgeDistance = 2f; // How far to move left

    // This is called by the FightManager to start the battle
    public void Setup(KaijuData data, FightManager manager)
    {
        kaijuData = data;
        fightManager = manager;

        currentHealth = kaijuData.maxHealth;
        healthSlider.maxValue = kaijuData.maxHealth;
        healthSlider.value = currentHealth;
        
        // Store our starting position
        originPosition = transform.position;
        
        if(powerUpGlow != null)
            powerUpGlow.SetActive(false);
    }

    // --- ACTIONS ---
    
    public void PerformMove(MoveData move)
    {
        if (isStunned || isDodging) return; // Can't attack while dodging

        // Tell the manager to handle the attack's logic (damage, effects)
        fightManager.HandleAttack(this, move);
    }
    
    // MODIFIED: This now starts a Coroutine for movement
    public void StartDodge()
    {
        if (isStunned || isDodging) return;
        
        // Start the dodge movement
        StartCoroutine(DodgeMovement());
    }

    // NEW: Coroutine for the dodge movement
    IEnumerator DodgeMovement()
    {
        isDodging = true;
        
        // Move Left
        transform.position -= new Vector3(dodgeDistance, 0, 0);
        
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        
        // Return to origin
        transform.position = originPosition;
        
        isDodging = false;
    }
    
    // DELETED: StartDefend() and StopDefend()
    
    public void ShowPowerUp(bool show)
    {
        if(powerUpGlow != null)
        {
            powerUpGlow.SetActive(show);
        }
        else if(show)
        {
            // This log will help debug if the glow is missing
            Debug.LogWarning(gameObject.name + " tried to show PowerUp, but no glow object is assigned!");
        }
    }

    // --- REACTIONS ---
    
    public void TakeDamage(int damage)
    {
        // Check for dodge
        if (isDodging)
        {
            Debug.Log(gameObject.name + " DODGED!");
            return; 
        }

        // We removed Defend, so we just take damage
        int damageTaken = damage;
        Debug.Log(gameObject.name + " took " + damageTaken + " damage!");
        
        currentHealth -= damageTaken;
        if (currentHealth < 0) currentHealth = 0;
        healthSlider.value = currentHealth;
        
        // Tell the manager to check if the fight is over
        fightManager.CheckForWin();
    }
}
