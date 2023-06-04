using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOverworldManagers : MonoBehaviour
{
    [SerializeField] GameObject gameManager;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Manager") == null)
        {
            Instantiate(gameManager);
        }
    }
}
