using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
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
    }

}
