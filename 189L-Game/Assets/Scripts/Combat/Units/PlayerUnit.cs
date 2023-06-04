using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public class PlayerUnit : GenericUnit
    {
        public PlayerUnit(ClassData classInfo, string name, float currentPlayerHP)
        {
            this.UnitName = name;
            this.BaseClassData = classInfo;
            this.CurrentHP = currentPlayerHP;
            this.MaxHP = classInfo.BaseHP;
            this.Attack = classInfo.BaseAttack;
            this.Defense = classInfo.BaseDefense;
            this.Agility = classInfo.BaseAgility;
        }

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
