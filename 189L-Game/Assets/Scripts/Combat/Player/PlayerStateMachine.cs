using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerUnit Player;
    public CombatStateMachine CSM;
    public GameObject EnemyToTarget = null;
    private bool actionStarted = false;

    public enum TurnState
    {
        WAIT,
        SELECTACTION,
        SELECTTARGET,
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
        switch(CurrentState)
        {
            case TurnState.WAIT:
                break;
            case TurnState.SELECTACTION:
                break;
            case TurnState.ATTACK:
                StartCoroutine(PerformAttack());
                break;
            case TurnState.DEAD:
                break;
        }
    }

    /*
    public void SelectAction(GameObject target)
    {
        // Randomly select a player unit to target.
        playerToTarget = CSM.AlliesInBattle[Random.Range(0, CSM.AlliesInBattle.Count)];
        CurrentState = TurnState.ATTACK;
    }*/

    private IEnumerator PerformAttack()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Animate player to attack enemy unit.
        var initialPosition = transform.position;
        var targetPosition = EnemyToTarget.transform.position - new Vector3(1f, 0f, 0f);
        while (MoveTowardsPosition(targetPosition))
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

        CSM.CurrentUIState = CombatStateMachine.UIStates.ACTIVATE;
        
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

