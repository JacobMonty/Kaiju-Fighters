using UnityEngine;
using TMPro;

[RequireComponent(typeof(KaijuController))]
public class PlayerInput : MonoBehaviour
{
    [Header("UI Cooldown Displays")]
    public TextMeshProUGUI attackCooldownText;
    public TextMeshProUGUI dodgeCooldownText;

    [Header("UI Base Text")]
    public string attackBaseText = "Attack >";
    public string dodgeBaseText = "< Dodge";

    [Header("Settings")]
    public float sharedCooldown = 1.0f; 
    
    private KaijuController controller;
    private FightManager fightManager;
    private float cooldownTimer = 0f;
    
    private MoveData kaijuAttack;

    void Start()
    {
        controller = GetComponent<KaijuController>();
        fightManager = FightManager.singleton;
        
        if (controller.kaijuData != null && controller.kaijuData.moves.Count > 0)
        {
            kaijuAttack = controller.kaijuData.moves[0]; 
        }
        else
        {
            Debug.LogError("Player has no moves assigned in its KaijuData!");
        }
        
        // Set the initial text from the public variables
        if (attackCooldownText != null)
            attackCooldownText.text = attackBaseText;
        if (dodgeCooldownText != null)
            dodgeCooldownText.text = dodgeBaseText;
    }

    void Update()
    {
        // 1. Always update the cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            
            string cdText = $"({(int)cooldownTimer + 1})";
            
            // Check both text objects individually
            if (attackCooldownText != null) 
                attackCooldownText.text = "Attack " + cdText;
            if (dodgeCooldownText != null) 
                dodgeCooldownText.text = "Dodge " + cdText;
        }
        else
        {
            // Reset both text objects individually
            if (attackCooldownText != null && attackCooldownText.text != attackBaseText)
                attackCooldownText.text = attackBaseText;
            if (dodgeCooldownText != null && dodgeCooldownText.text != dodgeBaseText)
                dodgeCooldownText.text = dodgeBaseText;
        }

        // 2. Don't allow any input if stunned, dodging, or battle is over
        if (controller.isStunned || controller.isDodging || fightManager.isBattleOver)
        {
            return; 
        }

        // 3. Handle Actions (only if cooldown is ready)
        if (cooldownTimer <= 0)
        {
            // Dodge (Left Arrow)
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                controller.StartDodge();
                cooldownTimer = sharedCooldown; // Start shared cooldown
            }
            // Attack (Right Arrow)
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (kaijuAttack != null)
                {
                    controller.PerformMove(kaijuAttack);
                    cooldownTimer = sharedCooldown; // Start shared cooldown
                }
            }
        }
    }
}
