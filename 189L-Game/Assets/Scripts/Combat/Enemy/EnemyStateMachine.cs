using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyUnit Enemy;
    public CombatStateMachine CSM;
    private bool actionStarted = false;
    private GameObject playerToTarget = null;
    
    public enum TurnState
    {
        WAIT,
        SELECTACTION,
        ATTACK,
        CLASSACTION,
        DEAD
    }

    public TurnState CurrentState;

    
    void Start()
    {
        CurrentState = TurnState.WAIT;
        CSM = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
    }

    void Update()
    {
        switch (CurrentState)
        {
            case TurnState.WAIT:
                break;
            case TurnState.SELECTACTION:
                SelectAction();
                break;
            case TurnState.ATTACK:
                StartCoroutine(PerformAttack());
                break;
            case TurnState.DEAD:
                break;
        }
    }

    private void SelectAction()
    {
        // Randomly select a player unit to target.
        playerToTarget = CSM.AlliesInBattle[Random.Range(0, CSM.AlliesInBattle.Count)];
        CurrentState = TurnState.ATTACK;
    }

    private IEnumerator PerformAttack()
    {
        if(actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Animate enemy to attack player unit.
        var initialPosition = transform.position;
        var targetPosition = playerToTarget.transform.position + new Vector3(1f, 0f, 0f);
        while(MoveTowardsPosition(targetPosition))
        {
            yield return null;
        }

        // Pause for 0.5 seconds.
        yield return new WaitForSeconds(0.5f);

        // Animate enemy back to initial position.
        while (MoveTowardsPosition(initialPosition))
        {
            yield return null;
        }

        // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
        CSM.EndTurn(this.gameObject);

        // Set combat state of CSM to Wait.
        CSM.CurrentCombatState = CombatStateMachine.CombatStates.WAIT;

        actionStarted = false;
        CurrentState = TurnState.WAIT;
    }

    private bool MoveTowardsPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime));
    }
}

