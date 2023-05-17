using System.Collections;
using System.Collections.Generic;
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
        ATTACKING,
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
            case TurnState.ATTACKING:
                StartCoroutine(PerformAttack());
                break;
            case TurnState.DEAD:
                break;
        }
    }

    private void SelectAction()
    {
        // Randomly select a player unit to target
        playerToTarget = CSM.AlliesInBattle[Random.Range(0, CSM.AlliesInBattle.Count)];
        CurrentState = TurnState.ATTACKING;
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

        // Remove this enemy game object from front of turn queue and readd back at the back of the queue.
        CSM.TurnOrder.RemoveAt(0);
        CSM.TurnOrder.Add(this.gameObject);

        // Set state of CSM to Wait.
        CSM.CurrentState = CombatStateMachine.CombatStates.WAIT;

        actionStarted = false;
        CurrentState = TurnState.WAIT;
    }

    private bool MoveTowardsPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime));
    }
}

