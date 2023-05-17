using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    public enum CombatStates
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public CombatStates CurrentState;

    // List containing player and enemy units, sorted according to position on battlefield.
    public List<GameObject> AlliesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();
    public List<GameObject> UnitsInBattle = new List<GameObject>();
    public List<GameObject> TurnOrder = new List<GameObject>();
    void Start()
    {
        AlliesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        
        UnitsInBattle.AddRange(AlliesInBattle);
        UnitsInBattle.AddRange(EnemiesInBattle);

        UnitsInBattle.Sort(delegate(GameObject x, GameObject y)
        {
            if (x.transform.position.x > y.transform.position.x) return 1;
            else return -1;
        });

        TurnOrder.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        TurnOrder.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        TurnOrder.Sort(delegate (GameObject x, GameObject y)
        {
            float xAgility, yAgility;

            if(x.tag == "Ally")
            {
                xAgility = x.GetComponent<PlayerStateMachine>().Player.Agility;
            }
            else
            {
                xAgility = x.GetComponent<EnemyStateMachine>().Enemy.Agility;
            }

            if (y.tag == "Ally")
            {
                yAgility = y.GetComponent<PlayerStateMachine>().Player.Agility;
            }
            else
            {
                yAgility = y.GetComponent<EnemyStateMachine>().Enemy.Agility;
            }

            if (xAgility < yAgility) return 1;
            else return -1;
        });

        CurrentState = CombatStates.TAKEACTION;
    }

    void Update()
    {
        switch(CurrentState)
        {
            case CombatStates.WAIT:
                if(TurnOrder.Count > 0)
                {
                    CurrentState = CombatStates.TAKEACTION;
                }
                break;
            case CombatStates.TAKEACTION:
                if (TurnOrder[0].tag == "Ally")
                {
                    var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
                    PSM.CurrentState = PlayerStateMachine.TurnState.SELECTACTION;
                }
                else if (TurnOrder[0].tag == "Enemy")
                {
                    var ESM = TurnOrder[0].GetComponent<EnemyStateMachine>();
                    ESM.CurrentState = EnemyStateMachine.TurnState.SELECTACTION;
                }
                else
                {
                    Debug.Log("Invalid tag!");
                }
                CurrentState = CombatStates.PERFORMACTION;
                break;
            case CombatStates.PERFORMACTION:
                break;
        }
    }
}
