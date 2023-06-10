using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class EnemyUnit : GenericUnit
    {
      public EnemyUnit(ClassData classInfo)
      {
          this.UnitName = classInfo.ClassName;
          this.BaseClassData = classInfo;
          this.CurrentHP = classInfo.BaseHP;
          this.MaxHP = classInfo.BaseHP;
          this.Attack = classInfo.BaseAttack;
          this.Defense = classInfo.BaseDefense;
          this.Agility = classInfo.BaseAgility;
      }
    }
}
