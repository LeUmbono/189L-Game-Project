using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "newEnemyParty", menuName = "Party/EnemyParty")]
public class EnemyPartyData : GenericPartyData
{
    public EnemyUnit slot1 = new EnemyUnit();
    public EnemyUnit slot2 = new EnemyUnit();
    public EnemyUnit slot3 = new EnemyUnit();
    public EnemyUnit slot4 = new EnemyUnit();
}
