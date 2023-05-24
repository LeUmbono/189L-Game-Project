using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatStateMachine : MonoBehaviour
{
    public enum CombatStates
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKGAME,
        WIN,
        LOSE,
    }

    public enum UIStates
    {
        ACTIVATE,
        WAIT,
        SELECTACTION,
        SELECTTARGET,
        DONE
    }

    public CombatStates CurrentCombatState;
    public UIStates CurrentUIState;

    public List<GameObject> AlliesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    // List containing player and enemy units, sorted according to position on battlefield.
    public List<GameObject> UnitsInBattle = new List<GameObject>();

    // List that tracks the turn order queue.
    public List<GameObject> TurnOrder = new List<GameObject>();

    public GameObject TargetIndicator;

    public GameObject UnitInfoPanel;
    public GameObject SelectActionPanel;
    public GameObject SelectTargetPanel;

    public List<GameObject> TargetButtons;

    private PlayerStateMachine.TurnState PlayerActionType;

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

        CurrentCombatState = CombatStates.TAKEACTION;
        CurrentUIState = UIStates.ACTIVATE;

        // Deactivate UI elements at the start of combat.
        UnitInfoPanel.SetActive(false);
        SelectActionPanel.SetActive(false);
        SelectTargetPanel.SetActive(false);
    }

    void Update()
    {
        switch(CurrentCombatState)
        {
            case CombatStates.WAIT:
                TargetIndicator.SetActive(false);
                if (TurnOrder.Count > 0)
                {
                    CurrentCombatState = CombatStates.TAKEACTION;
                }
                break;
            case CombatStates.TAKEACTION:
                TargetIndicator.transform.position = TurnOrder[0].transform.position 
                    + new Vector3(0.0f, 1.0f, 0.0f);
                TargetIndicator.SetActive(true);

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
                
                CurrentCombatState = CombatStates.PERFORMACTION;
                break;
            case CombatStates.PERFORMACTION:
                TargetIndicator.transform.position = TurnOrder[0].transform.position
                    + new Vector3(0.0f, 1.0f, 0.0f);
                break;
            case CombatStates.CHECKGAME:
                if(AlliesInBattle.Count < 1)
                {
                    // Disable target indicator.
                    TargetIndicator.SetActive(false);
                    CurrentUIState = UIStates.WAIT;
                    CurrentCombatState = CombatStates.LOSE;
                }
                else if(EnemiesInBattle.Count < 1)
                {
                    // Disable target indicator.
                    TargetIndicator.SetActive(false);
                    CurrentUIState = UIStates.WAIT;
                    CurrentCombatState = CombatStates.WIN;
                }
                else
                {
                    CurrentUIState = UIStates.ACTIVATE;
                    CurrentCombatState = CombatStates.WAIT;
                }
                break;
            case CombatStates.WIN:
                Debug.Log("You win!");
                break;
            case CombatStates.LOSE:
                Debug.Log("You lose!");
                break;
        }

        switch(CurrentUIState)
        {
            case UIStates.ACTIVATE:
                if (TurnOrder.Count > 0 && TurnOrder[0].tag == "Ally")
                {
                    UnitInfoPanel.SetActive(true);
                    SelectActionPanel.SetActive(true);
                    CurrentUIState = UIStates.WAIT;
                }
                break;
            case UIStates.WAIT:
                break;
            case UIStates.DONE:
                SelectionDone();
                break;
        }
    }


    public void EndTurn(GameObject unit)
    {
        TurnOrder.RemoveAt(0);
        TurnOrder.Add(unit);
    }

    public void SelectAttack()
    {
        PlayerActionType = PlayerStateMachine.TurnState.ATTACK;
        SelectActionPanel.SetActive(false);

        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
        
        // Only let player attack enemies up until their attack range.
        DisableTargetButtons();
        //EnableTargetButtons(4, Mathf.Min(PSM.Location + PSM.Player.BaseClassData.AttackRange, 7));
        EnableTargetButtons(0, 7);

        SelectTargetPanel.SetActive(true);
    }

    public void SelectSwap()
    {
        PlayerActionType = PlayerStateMachine.TurnState.SWAP;
        SelectActionPanel.SetActive(false);

        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();

        // Only let player swap adjacent units.
        DisableTargetButtons();
        EnableTargetButtons(Mathf.Max(PSM.Location-1, 0), Mathf.Min(PSM.Location+1, 3), PSM.Location);

        SelectTargetPanel.SetActive(true);
    }

    public void SelectSpecial()
    {

    }

    public void SelectTarget(GameObject target)
    {
        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
        PSM.UnitToTarget = target;
        CurrentUIState = UIStates.DONE;
    }

    private void SelectionDone()
    {
        UnitInfoPanel.SetActive(false);
        SelectTargetPanel.SetActive(false);
        
        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
        PSM.CurrentState = PlayerActionType;
    }

    private void DisableTargetButtons()
    {
        foreach(var button in TargetButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    private void EnableTargetButtons(int minRange, int maxRange, int targetSelf = -1)
    {
       for(int i = minRange; i <= maxRange; i++)
        {
            if(i != targetSelf)
            {
                TargetButtons[i].GetComponent<Button>().interactable = true;
            }
        }
    }
}
