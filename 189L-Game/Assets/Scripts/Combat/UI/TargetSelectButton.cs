using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class TargetSelectButton : MonoBehaviour
    {
        public GameObject TargetPrefab;

        public void SelectTarget()
        {
            GameObject.Find("CombatManager").GetComponent<CombatStateMachine>().SelectTarget(TargetPrefab);
        }
    }
}
