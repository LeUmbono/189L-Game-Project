using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerUnit Player;
    public CombatStateMachine CSM;

    public enum TurnState
    {
        WAIT,
        SELECTACTION,
        SELECTTARGET,
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
        switch(CurrentState)
        {
            case TurnState.WAIT:
                break;
            case TurnState.SELECTACTION:
                Debug.Log("Hi");
                CurrentState = TurnState.WAIT;
                break;
            case TurnState.ATTACKING: 
                break;
            case TurnState.DEAD:
                break;
        }
    }
}
