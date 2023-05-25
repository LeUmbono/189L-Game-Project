using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : ScriptableObject
{
    public virtual void Execute(GameObject gameObject)
    {

    }

    public virtual List<bool> SelectTargets()
    {
        return null;
    }
}