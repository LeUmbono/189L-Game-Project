using System.Collections.Generic;
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

    // List that tracks the turn order queue.
    public List<GameObject> TurnOrder = new List<GameObject>();

    [SerializeField] private GameObject targetIndicator;


    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private GameObject selectActionPanel;
    [SerializeField] private GameObject selectTargetPanel;

    public List<GameObject> TargetButtons;

    private GenericUnitStateMachine.TurnState PlayerActionType;

    void Start()
    {
        AlliesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

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
        unitInfoPanel.SetActive(false);
        selectActionPanel.SetActive(false);
        selectTargetPanel.SetActive(false);
    }

    void Update()
    {
        switch(CurrentCombatState)
        {
            case CombatStates.WAIT:
                targetIndicator.SetActive(false);
                if (TurnOrder.Count > 0)
                {
                    CurrentCombatState = CombatStates.TAKEACTION;
                }
                break;
            case CombatStates.TAKEACTION:
                targetIndicator.transform.position = TurnOrder[0].transform.position 
                    + new Vector3(0.0f, 1.0f, 0.0f);
                targetIndicator.SetActive(true);

                var GSM = TurnOrder[0].GetComponent<GenericUnitStateMachine>();
                GSM.CurrentState = GenericUnitStateMachine.TurnState.SELECTACTION;
                
                CurrentCombatState = CombatStates.PERFORMACTION;
                break;
            case CombatStates.PERFORMACTION:
                targetIndicator.transform.position = TurnOrder[0].transform.position
                    + new Vector3(0.0f, 1.0f, 0.0f);
                break;
            case CombatStates.CHECKGAME:
                if(AlliesInBattle.Count < 1)
                {
                    // Disable target indicator.
                    targetIndicator.SetActive(false);
                    CurrentUIState = UIStates.WAIT;
                    CurrentCombatState = CombatStates.LOSE;
                }
                else if(EnemiesInBattle.Count < 1)
                {
                    // Disable target indicator.
                    targetIndicator.SetActive(false);
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
                    unitInfoPanel.SetActive(true);
                    selectActionPanel.SetActive(true);
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
        PlayerActionType = GenericUnitStateMachine.TurnState.ATTACK;
        selectActionPanel.SetActive(false);

        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
        
        // Only let player attack enemies up until their attack range.
        DisableTargetButtons();
        
        var targets = new List<bool>() { false, false, false, false, false, false, false, false };

        for(int i = 4; i <= Mathf.Min(PSM.Location + PSM.Player.BaseClassData.AttackRange, 7); i++)
        { 
            targets[i] = true;
        }
        
        EnableTargetButtons(targets);

        selectTargetPanel.SetActive(true);
    }

    public void SelectSwap()
    {
        PlayerActionType = GenericUnitStateMachine.TurnState.SWAP;
        selectActionPanel.SetActive(false);

        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();

        // Only let player swap adjacent units.
        DisableTargetButtons();

        var targets = new List<bool>() { false, false, false, false, false, false, false, false };
        
        if(PSM.Location - 1 >= 0)
        {
            targets[PSM.Location - 1] = true;
        }

        if(PSM.Location + 1 < 4)
        {
            targets[PSM.Location + 1] = true;
        }

        EnableTargetButtons(targets);

        selectTargetPanel.SetActive(true);
    }

    public void SelectSpecial()
    {
        PlayerActionType = GenericUnitStateMachine.TurnState.SPECIAL;
        selectActionPanel.SetActive(false);

        var special = TurnOrder[0].GetComponent<PlayerStateMachine>().Player.BaseClassData.SpecialAbility;

        // Only let player swap adjacent units.
        DisableTargetButtons();
        EnableTargetButtons(special.SelectTargets());

        selectTargetPanel.SetActive(true);
    }

    public void SelectTarget(GameObject target)
    {
        var PSM = TurnOrder[0].GetComponent<PlayerStateMachine>();
        PSM.UnitToTarget = target;
        CurrentUIState = UIStates.DONE;
    }

    public void CancelAction()
    {
        selectTargetPanel.SetActive(false);
        selectActionPanel.SetActive(true);
    }

    private void SelectionDone()
    {
        unitInfoPanel.SetActive(false);
        selectTargetPanel.SetActive(false);
        
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

    private void EnableTargetButtons(List<bool> targets)
    {
        if(targets.Count != 8) 
        {
            Debug.Log("Invalid list passed.");
        }

        for(int i = 0; i < TargetButtons.Count; i++)
        {
            var isDead = TargetButtons[i].GetComponent<GenericUnitStateMachine>().IsDead;
            if (targets[i] == true && isDead != true) 
            {
                TargetButtons[i].GetComponent<Button>().interactable = true;
            }
        }
    }
}
