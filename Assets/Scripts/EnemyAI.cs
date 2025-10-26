using System.Collections;
using UnityEngine;

[RequireComponent(typeof(KaijuController))]
public class EnemyAI : MonoBehaviour
{
    private KaijuController controller;
    private FightManager fightManager;
    
    private enum AIState { Idle, PoweringUp }
    private AIState currentState;
    
    public float minIdleTime = 2.0f;
    public float maxIdleTime = 5.0f;

    void Awake()
    {
        controller = GetComponent<KaijuController>();
    }

    public void StartAI()
    {
        fightManager = FightManager.singleton; 
        currentState = AIState.Idle;
        
        if (controller == null || controller.healthSlider == null)
        {
            Debug.LogError("EnemyAI: Controller or HealthSlider is not set up!");
            return;
        }
        StartCoroutine(AIStateMachine());
    }

    IEnumerator AIStateMachine()
    {
        // Check for battle over flag
        while (controller != null && controller.healthSlider.value > 0 && !fightManager.isBattleOver)
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
        if (controller.kaijuData.moves == null || controller.kaijuData.moves.Count == 0)
        {
            Debug.LogError(controller.kaijuData.name + " has no moves! Returning to Idle.");
            currentState = AIState.Idle;
            yield break;
        }
        
        MoveData moveToUse = controller.kaijuData.moves[
            Random.Range(0, controller.kaijuData.moves.Count)];
        
        if (!fightManager.isBattleOver)
            fightManager.EnemyPowerUpAttack(moveToUse);
        
        yield return new WaitForSeconds(1.5f + moveToUse.damageDelay); 
        
        currentState = AIState.Idle;
    }
}

