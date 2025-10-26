using System.Collections;
using UnityEngine;

[RequireComponent(typeof(KaijuController))]
public class EnemyAI : MonoBehaviour
{
    private KaijuController controller;
    private FightManager fightManager;
    
    private enum AIState { Idle, PoweringUp, Attacking, Defending }
    private AIState currentState;
    
    public float minIdleTime = 2.0f;
    public float maxIdleTime = 5.0f;

    // Use Awake to get components, as it runs before Start()
    void Awake()
    {
        controller = GetComponent<KaijuController>();
        // MOVED: fightManager = FightManager.singleton;
    }

    // This is a NEW public function that the FightManager will call
    public void StartAI()
    {
        // MOVED HERE: We assign fightManager here, because we know
        // the singleton is ready when this function is called.
        fightManager = FightManager.singleton; 
        
        currentState = AIState.Idle;
        
        if (controller == null || controller.healthSlider == null)
        {
            Debug.LogError("EnemyAI: Controller or HealthSlider is not set up!");
            return;
        }
        
        // This line was moved from Start()
        StartCoroutine(AIStateMachine());
    }

    IEnumerator AIStateMachine()
    {
        while (controller != null && controller.healthSlider.value > 0)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    yield return StartCoroutine(IdleState());
                    break;
                case AIState.PoweringUp:
                    yield return StartCoroutine(PowerUpState());
                    break;
            }
            yield return null;
        }
        Debug.Log("EnemyAI: State machine stopped (Kaiju defeated or error).");
    }

    IEnumerator IdleState()
    {
        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        
        currentState = AIState.PoweringUp;
    }
    
    IEnumerator PowerUpState()
    {
        // --- MODIFIED: Added a check ---
        // First, check if we even have any moves
        if (controller.kaijuData.moves == null || controller.kaijuData.moves.Count == 0)
        {
            Debug.LogError(controller.kaijuData.name + " has no moves! Returning to Idle.");
            currentState = AIState.Idle;
            yield break; // Stop this coroutine
        }
        
        // Pick a random move
        MoveData moveToUse = controller.kaijuData.moves[
            Random.Range(0, controller.kaijuData.moves.Count)];
        
        fightManager.EnemyPowerUpAttack(moveToUse);
        
        yield return new WaitForSeconds(1.5f + moveToUse.damageDelay); 
        
        currentState = AIState.Idle;
    }
}

