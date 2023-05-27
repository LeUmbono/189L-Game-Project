using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UIStateMachine : MonoBehaviour
    { 
        public enum UIStates
        {
            ACTIVATE,
            WAIT,
            SELECTACTION,
            SELECTTARGET,
            DONE
        }

        public UIStates CurrentUIState;
        public List<GameObject> TargetButtons;

        [SerializeField] private GameObject unitInfoPanel;
        [SerializeField] private GameObject selectActionPanel;
        [SerializeField] private GameObject selectTargetPanel;

        private PlayerStateMachine psm;
        private CombatStateMachine csm;
        private GenericUnitStateMachine.TurnState PlayerActionType;
        
        void Start()
        {
            csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
            CurrentUIState = UIStates.ACTIVATE;

            // Deactivate UI elements at the start of combat.
            unitInfoPanel.SetActive(false);
            selectActionPanel.SetActive(false);
            selectTargetPanel.SetActive(false);
        }

        void Update()
        {
            switch (CurrentUIState)
            {
                case UIStates.ACTIVATE:
                    if (csm.TurnOrder.Count > 0 && csm.TurnOrder[0].tag == "Ally")
                    {
                        psm = csm.TurnOrder[0].GetComponent<PlayerStateMachine>();
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

        public void SelectAttack()
        {
            PlayerActionType = GenericUnitStateMachine.TurnState.ATTACK;
            selectActionPanel.SetActive(false);

            // Only let player attack enemies up until their attack range.
            DisableTargetButtons();

            var targets = new List<bool>() { false, false, false, false, false, false, false, false };

            for (int i = 4; i <= Mathf.Min(psm.Location + psm.Player.BaseClassData.AttackRange, 7); i++)
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

            // Only let player swap adjacent units.
            DisableTargetButtons();

            var targets = new List<bool>() { false, false, false, false, false, false, false, false };

            if (psm.Location - 1 >= 0)
            {
                targets[psm.Location - 1] = true;
            }

            if (psm.Location + 1 < 4)
            {
                targets[psm.Location + 1] = true;
            }

            EnableTargetButtons(targets);

            selectTargetPanel.SetActive(true);
        }

        public void SelectSpecial()
        {
            PlayerActionType = GenericUnitStateMachine.TurnState.SPECIAL;
            selectActionPanel.SetActive(false);

            var special = psm.Player.BaseClassData.SpecialAbility;

            // Only let player swap adjacent units.
            DisableTargetButtons();
            EnableTargetButtons(special.SelectTargets());

            selectTargetPanel.SetActive(true);
        }

        public void SelectTarget(GameObject target)
        {
            psm.UnitToTarget = target;
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

            psm.CurrentState = PlayerActionType;
        }

        private void DisableTargetButtons()
        {
            foreach (var button in TargetButtons)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }

        private void EnableTargetButtons(List<bool> targets)
        {
            if (targets.Count != 8)
            {
                Debug.Log("Invalid list passed.");
            }

            for (int i = 0; i < TargetButtons.Count; i++)
            {
                var isDead = csm.UnitsInBattle[i].GetComponent<GenericUnitStateMachine>().IsDead;
                if (targets[i] == true && isDead != true)
                {
                    TargetButtons[i].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

}
