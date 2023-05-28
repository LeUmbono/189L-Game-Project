using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Combat
{
    [CreateAssetMenu(fileName = "HealerHeal", menuName = "Special/HealerHeal")]
    public class HealerHeal : SpecialAbility
    {
        public override void Execute(GameObject gameObject)
        {
            var PSM = gameObject.GetComponent<PlayerStateMachine>();
            var target = PSM.UnitToTarget.GetComponent<PlayerStateMachine>();

            // Play animation.

            // Heal damage. 
            target.Player.CurrentHP += 0.2f * target.Player.MaxHP;
            if(target.Player.CurrentHP > target.Player.MaxHP)
            {
                target.Player.CurrentHP = target.Player.MaxHP;
            }

            target.UpdateHealthBar(target.Player.CurrentHP);
        }

        public override List<bool> SelectTargets()
        {
            var targets = new List<bool>() { true, true, true, true, false, false, false, false };

            return targets;
        }
    }

}