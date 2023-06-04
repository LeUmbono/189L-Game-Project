using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
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
    }
}
