using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public abstract class SpecialAbility : ScriptableObject
    {
        public AudioClip specialSound;
        public abstract void Execute(GameObject gameObject);

        public abstract List<bool> SelectTargets(GenericUnitStateMachine performer);

        public abstract float GetSteamBarChangeValue();
    }
}
