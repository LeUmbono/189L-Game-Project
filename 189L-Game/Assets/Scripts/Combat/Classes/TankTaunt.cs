using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "TankTaunt", menuName = "Special/TankTaunt")]
    public class TankTaunt : SpecialAbility
    {
        public override void Execute(GameObject gameObject)
        {
            var PSM = gameObject.GetComponent<PlayerStateMachine>();
            var target = PSM.UnitToTarget.GetComponent<EnemyStateMachine>();

            // Play sound.
            PSM.PlaySound(specialSound);

            // Taunt the enemy.
            target.IsTaunted = true;
            target.UnitToTarget = gameObject;
        }

        public override List<bool> SelectTargets(GenericUnitStateMachine performer)
        {
            return new List<bool>() { false, false, false, false, true, true, true, true };
        }

        public override float GetSteamBarChangeValue()
        {
            return -15.0f;
        }
    }
}

