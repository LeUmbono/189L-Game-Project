using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
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
        protected UIStateMachine uism;
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

        protected void DoSwap(GameObject target)
        {
            // Switch positions of player unit and swapped target.
            var initialPosition = transform.position;

            this.gameObject.transform.position = target.transform.position;
            target.transform.position = initialPosition;

            // Change name of this variable.
            var targetLocation = target.GetComponent<GenericUnitStateMachine>().location;

            // Switch prefabs of associated buttons.
            GameObject thisButtonPrefab = uism.TargetButtons[location].
                GetComponent<TargetSelectButton>().TargetPrefab;

            uism.TargetButtons[location].GetComponent<TargetSelectButton>().
                TargetPrefab = uism.TargetButtons[targetLocation].
                GetComponent<TargetSelectButton>().TargetPrefab;

            uism.TargetButtons[targetLocation].GetComponent<TargetSelectButton>().
                TargetPrefab = thisButtonPrefab;

            // Switch locations of player unit and swapped target
            var positionInBattle = csm.UnitsInBattle[location];
            csm.UnitsInBattle[location] = csm.UnitsInBattle[targetLocation];
            csm.UnitsInBattle[targetLocation] = positionInBattle;

            target.GetComponent<GenericUnitStateMachine>().location = location;
            location = targetLocation;
        }
    }
}
