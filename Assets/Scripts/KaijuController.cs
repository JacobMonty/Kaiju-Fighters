using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class KaijuController : MonoBehaviour
{
    [Header("Data")]
    public KaijuData kaijuData; 
    
    [Header("Live Stats")]
    public bool isDodging = false;
    public bool isStunned = false; 
    
    [Header("Components")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nametagText;
    
    private FightManager fightManager;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    private Coroutine shakeCoroutine;
    private Coroutine flickerCoroutine; // For flickering

    void Awake()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(KaijuData data, FightManager manager)
    {
        kaijuData = data;
        fightManager = manager;

        // Set up UI
        healthSlider.maxValue = kaijuData.maxHealth;
        healthSlider.value = kaijuData.maxHealth;
        if (healthText != null) healthText.text = $"{kaijuData.maxHealth} / {kaijuData.maxHealth}";
        if (nametagText != null) nametagText.text = kaijuData.kaijuName;
        
        // Ensure sprite is visible at setup
        spriteRenderer.enabled = true;
    }
    
    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }
    
    public void PerformMove(MoveData move)
    {
        if (isStunned) return;
        fightManager.HandleAttack(this, move);
    }
    
    public void StartDodge()
    {
        if (isStunned) return;
        
        isDodging = true;
        StartCoroutine(DodgeMovement());
        
        Invoke("StopDodge", 0.5f); 
    }

    void StopDodge()
    {
        isDodging = false;
    }

    IEnumerator DodgeMovement()
    {
        Vector3 targetPos = originalPosition + new Vector3(-1, 0, 0); // Move left
        float duration = 0.1f; // Quick move to target
        float timer = 0f;
        
        while(timer < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos; // Ensure it reaches target
        
        yield return new WaitForSeconds(0.3f); // Stay for a bit
        
        timer = 0f;
        while(timer < duration)
        {
            transform.position = Vector3.Lerp(targetPos, originalPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Return to origin
    }

    public void ShowPowerUp(bool show)
    {
        if (spriteRenderer == null) return;

        if (show)
        {
            if (flickerCoroutine != null)
                StopCoroutine(flickerCoroutine);
            flickerCoroutine = StartCoroutine(FlickerCoroutine());
        }
        else
        {
            if (flickerCoroutine != null)
                StopCoroutine(flickerCoroutine);
            spriteRenderer.enabled = true; // Ensure it's visible after flicker stops
        }
    }

    // Coroutine to make the SpriteRenderer flicker
    IEnumerator FlickerCoroutine()
    {
        while (true)
        {
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f); // Faster flicker
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void TakeDamage(int damage)
    {
        // Check for dodge
        if (isDodging)
        {
            fightManager.Log(kaijuData.kaijuName + " dodged the attack!");
            return; 
        }

        int damageTaken = damage;
        
        healthSlider.value -= damageTaken;
        if (healthText != null) healthText.text = $"{healthSlider.value} / {kaijuData.maxHealth}";
        
        // Start damage shake
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(DamageShake());

        fightManager.Log(kaijuData.kaijuName + " takes " + damageTaken + " damage!");

        // Tell the manager to check if the fight is over
        fightManager.CheckForWin();
    }
    
    IEnumerator DamageShake()
    {
        Vector3 shakeOffset = new Vector3(0.1f, 0, 0);
        for (int i = 0; i < 3; i++)
        {
            transform.position = originalPosition + shakeOffset;
            yield return new WaitForSeconds(0.05f);
            transform.position = originalPosition;
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = originalPosition;
    }
    
    // oroutine for fading out on death
    public IEnumerator FadeOut()
    {
        float duration = 1.0f;
        float timer = 0f;
        Color startColor = spriteRenderer.color;
        
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        gameObject.SetActive(false); // Finally deactivate
    }
}
