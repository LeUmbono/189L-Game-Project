using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class EnemyController : OverworldEntity
    {
        private SceneGameManager transitioner;

        private void Start()
        {
            transitioner = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Ally")
            {
                //other.gameObject.GetComponent<PlayerController>().DisableInput();
                PartyData allyParty = other.gameObject.GetComponent<PlayerController>().partyData;
                StartCoroutine(transitioner.LoadCombatScene(allyParty, this.partyData));
            }
        }

    }
}
