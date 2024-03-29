using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "HealerHeal", menuName = "Special/HealerHeal")]
    public class HealerHeal : SpecialAbility
    {
        public override void Execute(GameObject gameObject)
        {
            var PSM = gameObject.GetComponent<PlayerStateMachine>();
            var target = PSM.UnitToTarget.GetComponent<PlayerStateMachine>();

            // Play sound.
            PSM.PlaySound(specialSound);

            // Heal damage. 
            target.Unit.CurrentHP += 0.4f * target.Unit.MaxHP;
            if(target.Unit.CurrentHP > target.Unit.MaxHP)
            {
                target.Unit.CurrentHP = target.Unit.MaxHP;
            }

            // Update health bar of healed player unit.
            target.UpdateHealthBar(target.Unit.CurrentHP);
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

        public override float GetSteamBarChangeValue()
        {
            return 15.0f;
        }
    }
}
