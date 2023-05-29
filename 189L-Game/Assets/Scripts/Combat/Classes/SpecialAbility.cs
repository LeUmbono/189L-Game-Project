using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class SpecialAbility : ScriptableObject
    {
        public virtual void Execute(GameObject gameObject)
        {

        }

        public virtual List<bool> SelectTargets(GenericUnitStateMachine performer)
        {
            return null;
        }
    }
}
