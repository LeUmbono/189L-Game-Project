using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class SpecialAbility : ScriptableObject
    {
        public AudioClip specialSound;
        public virtual void Execute(GameObject gameObject)
        {

        }

        public virtual List<bool> SelectTargets(GenericUnitStateMachine performer)
        {
            return null;
        }

        public virtual float GetSteamBarChangeValue()
        {
            return 0.0f;
        }
    }
}
