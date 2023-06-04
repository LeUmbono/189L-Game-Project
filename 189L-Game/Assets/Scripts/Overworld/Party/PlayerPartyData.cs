using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "newPlayerParty", menuName = "Party/PlayerParty")]
public class PlayerPartyData : GenericPartyData
{
    public PlayerUnit slot1 = new PlayerUnit();
    public PlayerUnit slot2 = new PlayerUnit();
    public PlayerUnit slot3 = new PlayerUnit();
    public PlayerUnit slot4 = new PlayerUnit();
}
