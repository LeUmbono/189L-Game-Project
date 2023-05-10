using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "newClass", menuName = "Class/ClassData")]
public class ClassData : ScriptableObject
{
    [SerializeField] private string className;
    [SerializeField] private float baseHP;
    [SerializeField] private float baseAttack;
    [SerializeField] private float baseDefense;
    [SerializeField] private float baseAgility;
    [SerializeField] private int attackRange;
    // [SerializeField] private IClassAction classAction;

    public string ClassName => className;
    public float BaseHP => baseHP;
    public float BaseAttack => baseAttack;
    public float BaseDefense => baseDefense;
    public float BaseAgility => baseAgility;
    public int AttackRange => attackRange;

    // public IClassAction ClassAction;

}
