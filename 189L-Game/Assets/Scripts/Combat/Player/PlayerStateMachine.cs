using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerUnit Player;

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
    }

    void Update()
    {
        switch(CurrentState)
        {
            case TurnState.WAIT:
                break;
            case TurnState.SELECTACTION:
                break;
            case TurnState.ATTACKING: 
                break;
            case TurnState.DEAD:
                break;
        }
    }
}
