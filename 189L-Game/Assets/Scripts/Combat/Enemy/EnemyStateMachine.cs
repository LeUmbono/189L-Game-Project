using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyUnit Enemy;
    public CombatStateMachine CSM;

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
                break;
            case TurnState.ATTACKING:
                break;
            case TurnState.DEAD:
                break;
        }
    }

    private void SelectAction()
    {
        // Attack random player unit
        TurnManager currentTurn = new TurnManager();
        currentTurn.TurnTaker = Enemy.UnitName;
        currentTurn.TurnTakerGameObject = this.gameObject;

    }
}

