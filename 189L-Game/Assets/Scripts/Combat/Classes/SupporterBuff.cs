using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "SupporterBuff", menuName = "Special/SupporterBuff")]
    public class SupporterBuff : SpecialAbility
    {
        public override void Execute(GameObject gameObject)
        {
            var PSM = gameObject.GetComponent<PlayerStateMachine>();
            var target = PSM.UnitToTarget.GetComponent<PlayerStateMachine>();

            // Play animation.

            // Buff the unit's attack or defense.
            int buffSeed = Random.Range(0, 2);

            switch(buffSeed)
            {
                case 0:
                    target.Player.Attack += 0.2f * target.Player.BaseClassData.BaseAttack;
                    break;
                case 1:
                    target.Player.Defense += 0.2f * target.Player.BaseClassData.BaseDefense;
                    break;
                default:
                    Debug.Log("Invalid buff seed.");
                    break;
            }

            target.UnitToTarget = gameObject;
        }

        public override List<bool> SelectTargets(GenericUnitStateMachine performer)
        {
            var targets = new List<bool>() { false, false, false, false, false, false, false, false };

            // Set unit to the left as targetable.
            if (performer.Location - 1 >= 0)
            {
                targets[performer.Location - 1] = true;
            }

            // Set unit to the right as targetable.
            if (performer.Location + 1 < 4)
            {
                targets[performer.Location + 1] = true;
            }

            return targets;
        }

    }
}
