using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerUnit Player;
    public int Location;
    private CombatStateMachine csm;
    public GameObject UnitToTarget = null;
    private bool actionStarted = false;
    private bool isDead = false;
    public enum TurnState
    {
        WAIT,
        SELECTACTION,
        SELECTTARGET,
        ATTACK,
        SWAP,
        SPECIAL,
        DEAD
    }

    public TurnState CurrentState;

    void Start()
    {
        CurrentState = TurnState.WAIT;
        csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
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
            case TurnState.SWAP:
                StartCoroutine(PerformSwap());
                break;
            case TurnState.DEAD:
                if(isDead)
                {
                    return;
                }
                else
                {
                    // Make dead character invulnerable to enemy attack.
                    csm.AlliesInBattle.Remove(this.gameObject);

                    // Change sprite to reflect death / play death animation.
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    
                    isDead = true;

                    // Remove unit from turn list.
                    csm.TurnOrder.Remove(this.gameObject);
                }
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
    private void DoDamage()
    {
        UnitToTarget.GetComponent<EnemyStateMachine>().TakeDamage(Player.Attack);
    }

    public void TakeDamage(float damage)
    {
        var damageTaken = Mathf.Max(damage - Player.Defense, 1.0f);
        Player.CurrentHP -= damageTaken;

        if(Player.CurrentHP <= 0.0f)
        {
            Player.CurrentHP = 0.0f;
            CurrentState = TurnState.DEAD;
        }

    }
    private IEnumerator PerformAttack()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Animate player to attack enemy unit.
        var initialPosition = transform.position;
        var targetPosition = UnitToTarget.transform.position - new Vector3(1f, 0f, 0f);
        while (MoveTowardsPosition(targetPosition))
        {
            yield return null;
        }

        // Pause for 0.5 seconds.
        yield return new WaitForSeconds(0.5f);

        // Do damage.
        DoDamage();

        // Animate enemy back to initial position.
        while (MoveTowardsPosition(initialPosition))
        {
            yield return null;
        }

        // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
        csm.EndTurn(this.gameObject);

        // Set combat state of CSM to CheckGame.
        csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

        //csm.CurrentUIState = CombatStateMachine.UIStates.ACTIVATE;

        actionStarted = false;
        CurrentState = TurnState.WAIT;
    }

    private IEnumerator PerformSwap()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Switch positions of player unit and swapped target.
        var initialPosition = transform.position;
        var targetPosition = UnitToTarget.transform.position;

        this.gameObject.transform.position = targetPosition;
        UnitToTarget.transform.position = initialPosition;

        // Change name of this variable.
        var targetLocation = UnitToTarget.GetComponent<PlayerStateMachine>().Location;

        // Switch prefabs of associated buttons.
        GameObject thisButtonPrefab = csm.TargetButtons[Location].GetComponent<TargetSelectButton>().TargetPrefab;
        GameObject targetButtonPrefab = csm.TargetButtons[targetLocation].GetComponent<TargetSelectButton>().TargetPrefab;

        csm.TargetButtons[Location].GetComponent<TargetSelectButton>().TargetPrefab = targetButtonPrefab;
        csm.TargetButtons[targetLocation].GetComponent<TargetSelectButton>().TargetPrefab = thisButtonPrefab;
        // Switch locations of player unit and swapped target
        UnitToTarget.GetComponent<PlayerStateMachine>().Location = Location;
        Location = targetLocation;

        // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
        csm.EndTurn(this.gameObject);

        // Set combat state of CSM to Wait.
        csm.CurrentCombatState = CombatStateMachine.CombatStates.WAIT;
        csm.CurrentUIState = CombatStateMachine.UIStates.ACTIVATE;

        actionStarted = false;
        CurrentState = TurnState.WAIT;
    }
    private bool MoveTowardsPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime));
    }


}

