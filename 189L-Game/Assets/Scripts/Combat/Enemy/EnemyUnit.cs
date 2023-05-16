using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit 
{
    // Information of enemy unit, specifically name and base stats from class.
    public string UnitName;
    public ClassData BaseClassData;

    // Run-time stats of enemy unit.
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public float Attack { get; private set; }
    public float Defense { get; private set; }
    public float Agility { get; private set; }

    public EnemyUnit() { }
}
