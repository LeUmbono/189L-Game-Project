using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangerSnipe", menuName = "Special/RangerSnipe")]
public class RangerSnipe : SpecialAbility
{  

    public override void Execute(GameObject gameObject)
    {
        var PSM = gameObject.GetComponent<PlayerStateMachine>();
        var target = PSM.UnitToTarget.GetComponent<EnemyStateMachine>();

        // Play animation.

        // Deal damage. 
        target.TakeDamage(PSM.Player.Attack * 2);
    }

    public override List<bool> SelectTargets()
    {
        return new List<bool>() { false, false, false, false, true, true, true, true};
    }
}