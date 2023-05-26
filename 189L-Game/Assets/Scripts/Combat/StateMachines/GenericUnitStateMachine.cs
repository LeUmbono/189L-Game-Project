using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUnitStateMachine : MonoBehaviour
{
    // Unit state list.
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

    // Run-time information about the unit.
    public TurnState CurrentState;
    public GameObject UnitToTarget;

    [SerializeField]
    protected int location;
    protected CombatStateMachine csm;
    protected bool actionStarted = false;
    protected bool isDead = false;

    // Getters.
    public int Location => location;
    public bool IsDead => isDead;   
    
    public virtual void TakeDamage(float damage)
    {

    }
    protected virtual void DoDamage()
    {
        
    }
}
