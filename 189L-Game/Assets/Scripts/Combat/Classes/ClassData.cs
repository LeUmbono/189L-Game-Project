using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "newClass", menuName = "Class/ClassData")]
    public class ClassData : ScriptableObject
    {
        // Class information.
        [SerializeField] private string className;
        [SerializeField] private Sprite classSprite;
        [SerializeField] private Sprite classIcon;

        // Base stats.
        [SerializeField] private float baseHP;
        [SerializeField] private float baseAttack;
        [SerializeField] private float baseDefense;
        [SerializeField] private float baseAgility;
        [SerializeField] private int attackRange;
        [SerializeField] private SpecialAbility specialAbility;

        // Getters for class information and stats.
        public string ClassName => className;
        public Sprite ClassSprite => classSprite;
        public Sprite ClassIcon => classIcon;
        public float BaseHP => baseHP;
        public float BaseAttack => baseAttack;
        public float BaseDefense => baseDefense;
        public float BaseAgility => baseAgility;
        public int AttackRange => attackRange;
        public SpecialAbility SpecialAbility => specialAbility;
    }
}
