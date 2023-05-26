using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUnitStateMachine : MonoBehaviour
{
    // Run-time information about the unit.
    [SerializeField]
    protected int location;
    protected CombatStateMachine csm;
    
    protected bool actionStarted = false;
    protected bool isDead = false;

    public int Location => location;
    public GameObject UnitToTarget;

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

    // Current state of the unit.
    public TurnState CurrentState;

    public virtual void TakeDamage(float damage)
    {

    }
    protected virtual void DoDamage()
    {
        
    }
}
