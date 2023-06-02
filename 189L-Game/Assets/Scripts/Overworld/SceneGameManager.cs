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
    private PartyData playerData;
    private PartyData enemyData;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator LoadCombatScene(PartyData pData, PartyData eData)
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
        int pos = 1;
        foreach (GameObject ally in allies)
        {
            switch(pos)
            {
                case 1:
                    ally.GetComponent<PlayerStateMachine>().Player.BaseClassData = playerData.slot1;
                    pos++;
                    break;
                case 2:
                    ally.GetComponent<PlayerStateMachine>().Player.BaseClassData = playerData.slot2;
                    pos++;
                    break;
                case 3:
                    ally.GetComponent<PlayerStateMachine>().Player.BaseClassData = playerData.slot3;
                    pos++;
                    break;
                case 4:
                    ally.GetComponent<PlayerStateMachine>().Player.BaseClassData = playerData.slot4;
                    pos++;
                    break;
                default:
                    Debug.LogError("TOO MANY ALLIES");
                    break;
            }
        }

        pos = 1;
        foreach (GameObject enemy in enemies)
        {
            switch(pos)
            {
                case 1:
                    enemy.GetComponent<EnemyStateMachine>().Enemy.BaseClassData = enemyData.slot1;
                    pos++;
                    break;
                case 2:
                    enemy.GetComponent<EnemyStateMachine>().Enemy.BaseClassData = enemyData.slot2;
                    pos++;
                    break;
                case 3:
                    enemy.GetComponent<EnemyStateMachine>().Enemy.BaseClassData = enemyData.slot3;
                    pos++;
                    break;
                case 4:
                    enemy.GetComponent<EnemyStateMachine>().Enemy.BaseClassData = enemyData.slot4;
                    pos++;
                    break;
                default:
                    Debug.LogError("TOO MANY ENEMIES");
                    break;
            }
        }
    }
}
