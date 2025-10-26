using UnityEngine;
using TMPro;

[RequireComponent(typeof(KaijuController))] // Renamed
public class PlayerInput : MonoBehaviour
{
    public TextMeshProUGUI attackCooldownText;
    public TextMeshProUGUI dodgeCooldownText; // NEW: For the dodge display
    
    private KaijuController controller; // Renamed
    private float actionCooldownTimer = 0f; // This is now a SHARED cooldown
    
    private MoveData kaijuAttack;
    public float dodgeCooldown = 1.5f; // Public variable for dodge cooldown
    
    private string attackBaseText;
    private string dodgeBaseText;

    void Start()
    {
        controller = GetComponent<KaijuController>(); // Renamed
        
        if (controller.kaijuData != null && controller.kaijuData.moves.Count > 0)
        {
            kaijuAttack = controller.kaijuData.moves[0]; 
        }
        else
        {
            Debug.LogError("Player has no moves assigned in its KaijuData!");
        }
        
        // Store the button's starting text
        if (attackCooldownText != null)
        {
            attackBaseText = attackCooldownText.text;
        }
        if (dodgeCooldownText != null)
        {
            dodgeBaseText = dodgeCooldownText.text;
        }
    }

    void Update()
    {
        // 1. Always update the SHARED cooldown timer
        if (actionCooldownTimer > 0)
        {
            actionCooldownTimer -= Time.deltaTime;
            
            // Update BOTH text fields
            string cooldownStr = $"{(int)actionCooldownTimer + 1}";
            if (attackCooldownText != null) attackCooldownText.text = cooldownStr;
            if (dodgeCooldownText != null) dodgeCooldownText.text = cooldownStr;
        }
        else
        {
            // Reset both texts
            if (attackCooldownText != null && attackCooldownText.text != attackBaseText)
            {
                attackCooldownText.text = attackBaseText;
            }
            if (dodgeCooldownText != null && dodgeCooldownText.text != dodgeBaseText)
            {
                dodgeCooldownText.text = dodgeBaseText;
            }
        }

        // 2. Don't allow any input if stunned or...
        // ...if we are already dodging (from the coroutine)
        // ...if the shared cooldown is active
        if (controller.isStunned || controller.isDodging || actionCooldownTimer > 0)
        {
            return;
        }

        // 3. Handle Actions (NEW Controls)
        
        // Dodge (Left Arrow)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            controller.StartDodge();
            actionCooldownTimer = dodgeCooldown; // Set shared cooldown
        }
        // Attack (Right Arrow)
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (kaijuAttack != null)
            {
                controller.PerformMove(kaijuAttack);
                actionCooldownTimer = kaijuAttack.cooldown; // Set shared cooldown
            }
        }
    }
}

