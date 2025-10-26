using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // For Text

public class FightManager : MonoBehaviour
{
    public KaijuController player;
    public KaijuController enemy;

    public KaijuData playerData; 
    public KaijuData enemyData;
    
    public string overworldSceneName = "Overworld";
    public string mainMenuSceneName = "EndScreen";

    public static FightManager singleton;
    
    [Header("UI References")]
    public Slider enemyHealthSlider;
    public Slider playerHealthSlider;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerNametagText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI enemyNametagText;
    public TextMeshProUGUI battleLogText;
    
    public bool isBattleOver = false; 
    private Coroutine logClearCoroutine; 

    void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
            return;
        }
        singleton = this;
    }

    void Start()
    {
        // 1. Assign UI components FIRST
        player.healthText = playerHealthText;
        player.nametagText = playerNametagText;
        enemy.healthText = enemyHealthText;
        enemy.nametagText = enemyNametagText;

        // 2. NOW call Setup()
        player.Setup(playerData, this);
        enemy.Setup(enemyData, this);
        
            
        StartCoroutine(StartBattleSequence());
    }
    
    IEnumerator StartBattleSequence()
    {
        // Fighters are now visible from the start
        if (battleLogText != null) battleLogText.text = "";
        
        Log("A wild " + enemy.kaijuData.kaijuName + " appears!");
        
        yield return new WaitForSeconds(2.5f);
        
        if (enemy.GetComponent<SpriteRenderer>() != null)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = true;
        }

        Log(player.kaijuData.kaijuName + " prepares for battle!");
        
        yield return new WaitForSeconds(2.0f);
        Log(""); 
        
        enemy.GetComponent<EnemyAI>().StartAI();
    }

    public void HandleAttack(KaijuController attacker, MoveData move)
    {
        if (isBattleOver) return;
        StartCoroutine(AttackCoroutine(attacker, move));
    }

    private IEnumerator AttackCoroutine(KaijuController attacker, MoveData move)
    {
        // 1. Get position and move back
        Vector3 startPos = attacker.GetOriginalPosition();
        Vector3 forwardDir = (attacker == player) ? Vector3.up : Vector3.down;
        
        attacker.transform.position = startPos - forwardDir * 0.5f; // Move back
        yield return new WaitForSeconds(0.3f); // Charge
        
        // 2. Lunge forward
        attacker.transform.position = startPos + forwardDir * 0.5f; // Lunge
        Log(attacker.kaijuData.kaijuName + " uses " + move.moveName + "!");
        
        if (move.particleEffect != null)
        {
            Instantiate(move.particleEffect, attacker.transform.position, Quaternion.identity);
        }
        
        // 3. Wait for damage delay
        yield return new WaitForSeconds(move.damageDelay);
        
        KaijuController target = (attacker == player) ? enemy : player;
        
        if (target == enemy && !target.isDodging) // Only if player attacks enemy
        {
            // Roll for a dodge
            float dodgeRoll = Random.Range(0, 100);
            if (dodgeRoll < target.kaijuData.dodgeChance)
            {
                // Tell the enemy to perform its dodge action
                target.StartDodge(); 
            }
        }

        // 4. Apply damage (TakeDamage function will check if dodging)
        target.TakeDamage(move.damage);
        
        // 5. Return to position
        yield return new WaitForSeconds(0.2f); // Hold lunge
        attacker.transform.position = startPos;
    }
    
    public void EnemyPowerUpAttack(MoveData move)
    {
        if (isBattleOver) return;
        StartCoroutine(EnemyAttackCoroutine(move));
    }
    
    private IEnumerator EnemyAttackCoroutine(MoveData move)
    {
        enemy.ShowPowerUp(true); // This will now start sprite flickering
        Log(enemy.kaijuData.kaijuName + " is charging an attack!");
        
        yield return new WaitForSeconds(1.5f);
        
        enemy.ShowPowerUp(false); // This will stop sprite flickering
        
        HandleAttack(enemy, move);
    }
    
    // Removed all references to gameOverPanel and gameOverText
    public void CheckForWin()
    {
        if (isBattleOver) return; 

        if (player.healthSlider.value <= 0)
        {
            isBattleOver = true;
            Log("You were defeated by " + enemy.kaijuData.kaijuName + "!");
            StartCoroutine(player.FadeOut()); // Player fades
            
            Invoke("GoToMainMenu", 4f); // Wait 4s
        }
        else if (enemy.healthSlider.value <= 0)
        {
            isBattleOver = true;
            Log(enemy.kaijuData.kaijuName + " was defeated!");
            StartCoroutine(enemy.FadeOut()); // Enemy fades
            
            Invoke("GoToOverworld", 4f); // Wait 4s
        }
    }
    
    void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    void GoToOverworld()
    {
        SceneManager.LoadScene(overworldSceneName);
    }
    
    public void Log(string message)
    {
        if (battleLogText == null) return;
        
        if (logClearCoroutine != null)
            StopCoroutine(logClearCoroutine);
            
        battleLogText.text = message;
        
        if (!isBattleOver)
            logClearCoroutine = StartCoroutine(ClearLogAfterDelay());
    }
    
    IEnumerator ClearLogAfterDelay()
    {
        yield return new WaitForSeconds(2.0f); 
        if (battleLogText != null)
            battleLogText.text = "";
    }
}

