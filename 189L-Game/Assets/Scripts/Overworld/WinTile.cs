using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTile : MonoBehaviour
{
    private SceneGameManager sgm;

    private void Awake()
    {
        sgm = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Won game!");
            StartCoroutine(sgm.LoadTitleScene());
        }
    }
}
