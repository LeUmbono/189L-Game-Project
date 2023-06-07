using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "newParty", menuName = "Party/NewParty")]
public class PartyData : ScriptableObject
{
    public ClassData slot1;
    public ClassData slot2;
    public ClassData slot3;
    public ClassData slot4;

    public float slot1curHP;
    public float slot2curHP;
    public float slot3curHP;
    public float slot4curHP;

    public string slot1name;
    public string slot2name;
    public string slot3name;
    public string slot4name;

    /*
    public PartyData()
    {
        slot1curHP = slot1.BaseHP;
        slot2curHP = slot2.BaseHP;
        slot3curHP = slot3.BaseHP;
        slot4curHP = slot4.BaseHP;
    }
    */
    public void FullHeal()
    {
        slot1curHP = slot1.BaseHP;
        slot2curHP = slot2.BaseHP;
        slot3curHP = slot3.BaseHP;
        slot4curHP = slot4.BaseHP;
    }
}
