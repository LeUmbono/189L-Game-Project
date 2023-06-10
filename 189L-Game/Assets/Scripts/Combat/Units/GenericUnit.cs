using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public class GenericUnit
    {
        // Information of unit, specifically name and base stats from class.
        public string UnitName;
        public ClassData BaseClassData;

        // Run-time stats of unit.
        public float CurrentHP; 
        public float MaxHP; 
        public float Attack; 
        public float Defense; 
        public float Agility;

        public void ResetStats()
        {
            Attack = BaseClassData.BaseAttack;
            Defense = BaseClassData.BaseDefense;
            Agility = BaseClassData.BaseAgility;
        }

        public void ApplyMultiplierToStats(float atkMultiplier, float defMultiplier, float agiMultiplier)
        {
            Attack *= atkMultiplier;
            Defense *= defMultiplier;
            Agility *= atkMultiplier;
        }
    }
}
