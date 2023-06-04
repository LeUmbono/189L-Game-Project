using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using Overworld;

public class SceneGameManager : MonoBehaviour
{
    [SerializeField] private string combatSceneName;
    [SerializeField] private float timeToWait;
    [SerializeField][Range(0, 1)] private float slowdownPercent;

    //Holds the most recent reference to player and enemy
    public PlayerPartyData playerData;
    public EnemyPartyData enemyData;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerData = GameObject.FindWithTag("Ally").GetComponent<PlayerController>().partyData;
    }

    public IEnumerator LoadCombatScene(PlayerPartyData pData, EnemyPartyData eData)
    {
        Debug.Log("Play Transition");
        Time.timeScale = slowdownPercent;
        yield return new WaitForSeconds(timeToWait * slowdownPercent);
        SceneManager.LoadScene(combatSceneName, LoadSceneMode.Single);
        Time.timeScale = 1f;

        playerData = pData;
        enemyData = eData;
    }

    public void InitializeCombatScene()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //initializes from top to bottom based on scene
        foreach (GameObject ally in allies)
        {
            PlayerStateMachine psm = ally.GetComponent<PlayerStateMachine>();
            switch(psm.Location)
            {
                case 0:
                    psm.Player = this.playerData.slot1;
                    break;
                case 1:
                    psm.Player = this.playerData.slot2;
                    break;
                case 2:
                    psm.Player = this.playerData.slot3;
                    break;
                case 3:
                    psm.Player = this.playerData.slot4;
                    break;
                default:
                    Debug.LogError("NO PROPER LOCATION");
                    break;
          }
        }

        foreach (GameObject enemy in enemies)
        {
            EnemyStateMachine esm = enemy.GetComponent<EnemyStateMachine>();
            switch(esm.Location)
            {
                case 4:
                    esm.Enemy = this.enemyData.slot1;
                    break;
                case 5:
                    esm.Enemy = this.enemyData.slot2;
                    break;
                case 6:
                    esm.Enemy = this.enemyData.slot3;
                    break;
                case 7:
                    esm.Enemy = this.enemyData.slot4;
                    break;
                default:
                    Debug.LogError("NO PROPER LOCATION");
                    break;
            }
        }
        HealEnemyParty();

    }

    private void HealPlayerParty()
    {
        this.playerData.slot1.FullHeal();
        this.playerData.slot2.FullHeal();
        this.playerData.slot3.FullHeal();
        this.playerData.slot4.FullHeal();
    }

    private void HealEnemyParty()
    {
        this.enemyData.slot1.FullHeal();
        this.enemyData.slot2.FullHeal();
        this.enemyData.slot3.FullHeal();
        this.enemyData.slot4.FullHeal();
    }
}
