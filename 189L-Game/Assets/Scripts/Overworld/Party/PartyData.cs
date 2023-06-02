using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "newParty", menuName = "Class/PartyData")]
public class PartyData : ScriptableObject
{
    public ClassData slot1;
    public ClassData slot2;
    public ClassData slot3;
    public ClassData slot4;
}
