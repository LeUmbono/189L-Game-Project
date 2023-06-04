using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public class PlayerUnit : GenericUnit
    {
        public PlayerUnit() { }

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
