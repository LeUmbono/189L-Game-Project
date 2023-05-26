using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyUnit Enemy;
    public int Location;
    private CombatStateMachine csm;
    private GameObject playerToTarget = null;
    private bool actionStarted = false;
    private bool isDead = false;
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
        csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
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
                if (isDead)
                {
                    return;
                }
                else
                {
                    // Make dead character invulnerable to enemy attack.
                    csm.EnemiesInBattle.Remove(this.gameObject);

                    // Change sprite to reflect death / play death animation.
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;

                    isDead = true;

                    // Remove unit from turn list.
                    csm.TurnOrder.Remove(this.gameObject);
;
                }
                break;
        }
    }

    private void SelectAction()
    {
        // Randomly select a player unit to target.
        playerToTarget = csm.AlliesInBattle[Random.Range(0, csm.AlliesInBattle.Count)];
        CurrentState = TurnState.ATTACK;
    }
    
    private void DoDamage()
    {
        playerToTarget.GetComponent<PlayerStateMachine>().TakeDamage(Enemy.Attack);
    }

    public void TakeDamage(float damage)
    {
        var damageTaken = Mathf.Max(damage - Enemy.Defense, 1.0f);
        Enemy.CurrentHP -= damageTaken;

        if (Enemy.CurrentHP <= 0.0f)
        {
            Enemy.CurrentHP = 0.0f;
            CurrentState = TurnState.DEAD;
        }
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

        actionStarted = false;
        CurrentState = TurnState.WAIT;
    }

    private bool MoveTowardsPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime));
    }
}
