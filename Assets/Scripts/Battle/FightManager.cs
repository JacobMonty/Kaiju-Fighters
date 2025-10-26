using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // NEW

public class FightManager : MonoBehaviour
{
    // MODIFIED: We reference the controllers, not their stats
    public KaijuController player;
    public KaijuController enemy;

    // NEW: Assign these in the Inspector
    public KaijuData playerData; 
    public KaijuData enemyData;
    
    // NEW: For win/loss
    public string overworldSceneName = "Overworld"; // Set in inspector
    public string mainMenuSceneName = "MainMenu"; // Set in inspector
    public GameObject gameOverPanel; // NEW: Link your game over UI

    public static FightManager singleton;
    
    // DELETED: animPlayer, animEnemy, playerHealth, enemyHealth, etc.
    // We keep the sliders to pass them to the controllers
    public Slider enemyHealthSlider;
    public Slider playerHealthSlider;

    void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
            return;
        }
        singleton = this;
    }

    // NEW: Start is now used to set up the battle
    void Start()
    {
        // Get the animators from the controllers
        // Animator playerAnim = player.GetComponent<Animator>(); // ANIMATION REMOVED
        // Animator enemyAnim = enemy.GetComponent<Animator>(); // ANIMATION REMOVED

        // Pass the sliders and data to the controllers
        player.Setup(playerData, this);
        enemy.Setup(enemyData, this);

        // Pass animators to controllers
        // player.anim = playerAnim; // ANIMATION REMOVED
        // enemy.anim = enemyAnim; // ANIMATION REMOVED

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
            
        enemy.GetComponent<EnemyAI>().StartAI();
    }
    
    // DELETED: UpdateSliders() (Controller handles its own slider)
    // DELETED: StartEnemyAnimation() and StartPlayerAnimation() (Replaced)

    // --- NEW BATTLE LOGIC ---

    // This is called by the KaijuController when it attacks
    public void HandleAttack(KaijuController attacker, MoveData move)
    {
        StartCoroutine(AttackCoroutine(attacker, move));
    }

    private IEnumerator AttackCoroutine(KaijuController attacker, MoveData move)
    {
        // 1. Attacker plays animation (already started in KaijuController)
        // 2. Spawn particle effects
        if (move.particleEffect != null)
        {
            // You'll need to position this properly
            Instantiate(move.particleEffect, attacker.transform.position, Quaternion.identity);
        }

        // 3. Wait for the damage to apply (based on the move)
        yield return new WaitForSeconds(move.damageDelay);

        // 4. Determine the target
        KaijuController target = (attacker == player) ? enemy : player;

        // 5. AI Reaction Check (if enemy is target)
        if (target == enemy)
        {
            float dodgeRoll = Random.Range(0, 100);
            if (dodgeRoll < target.kaijuData.dodgeChance)
            {
                target.StartDodge();
            }
        }
        
        // 6. Apply damage
        target.TakeDamage(move.damage);
    }
    
    // NEW: This is for the enemy's "power up" attack
    public void EnemyPowerUpAttack(MoveData move)
    {
        StartCoroutine(EnemyAttackCoroutine(move));
    }
    
    private IEnumerator EnemyAttackCoroutine(MoveData move)
    {
        // 1. Signal the attack (power up)
        enemy.ShowPowerUp(true);
        
        // 2. Wait 1.5 seconds (gives player time to react)
        yield return new WaitForSeconds(1.5f);
        
        // 3. Attack!
        enemy.ShowPowerUp(false);
        enemy.PerformMove(move); // This will call HandleAttack
    }
    
    // NEW: Check for win/loss
    public void CheckForWin()
    {
        // Using the internal health from the controller
        if (player.healthSlider.value <= 0)
        {
            // Player lost
            if(gameOverPanel != null) gameOverPanel.SetActive(true);
            // After a delay, go to main menu
            Invoke("GoToMainMenu", 3f);
        }
        else if (enemy.healthSlider.value <= 0)
        {
            // Player won
            // Play a win animation, etc.
            // After a delay, go to overworld
            Invoke("GoToOverworld", 3f);
        }
    }
    
    void GoToMainMenu()
    {
        Time.timeScale = 1f; // Unpause the game before changing scenes
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    void GoToOverworld()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(overworldSceneName);
    }

    // We keep this helper function
    IEnumerator Delay(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
