using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using Overworld;

public class SceneGameManager : MonoBehaviour
{
    [SerializeField] private string combatSceneName;
    [SerializeField] private string overworldSceneName;
    [SerializeField] private string titleSceneName;
    [SerializeField] private float timeToWait;
    [SerializeField][Range(0, 1)] private float slowdownPercent;

    //Holds Reference to OverworldPlayer
    private PlayerController overworldPlayer;

    //Holds the most recent reference to player and enemy
    public PartyData playerData;
    public PartyData enemyData;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerData = GameObject.FindWithTag("Ally").GetComponent<PlayerController>().partyData;
        overworldPlayer = GameObject.FindWithTag("Ally").GetComponent<PlayerController>();
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

    //To only be called on from overworld / after combat
    public void UpdatePlayerData()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach(GameObject ally in allies)
        {
            PlayerStateMachine psm = ally.GetComponent<PlayerStateMachine>();
            switch(psm.Location)
            {
                case 0:
                    this.playerData.slot1curHP = psm.Player.CurrentHP;
                    break;
                case 1:
                    this.playerData.slot2curHP = psm.Player.CurrentHP;
                    break;
                case 2:
                    this.playerData.slot3curHP = psm.Player.CurrentHP;
                    break;
                case 3:
                    this.playerData.slot4curHP = psm.Player.CurrentHP;
                    break;
                default:
                    Debug.LogError("NO PROPER LOCATION");
                    break;
            }
        }
    }

    public IEnumerator LoadOverworldScene()
    {
        Debug.Log("Play Transition");
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(overworldSceneName, LoadSceneMode.Single);
        overworldPlayer.partyData = playerData;
    }

    public IEnumerator LoadTitleScene()
    {
      Debug.Log("Play Transition");
      yield return new WaitForSeconds(timeToWait);
      SceneManager.LoadScene(overworldSceneName, LoadSceneMode.Single);
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
                    psm.Player = new PlayerUnit(this.playerData.slot1, this.playerData.slot1name, this.playerData.slot1curHP);
                    break;
                case 1:
                    psm.Player = new PlayerUnit(this.playerData.slot2, this.playerData.slot2name, this.playerData.slot2curHP);
                    break;
                case 2:
                    psm.Player = new PlayerUnit(this.playerData.slot3, this.playerData.slot3name, this.playerData.slot3curHP);
                    break;
                case 3:
                    psm.Player = new PlayerUnit(this.playerData.slot4, this.playerData.slot4name, this.playerData.slot4curHP);
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
                    esm.Enemy = new EnemyUnit(this.enemyData.slot1);
                    break;
                case 5:
                    esm.Enemy = new EnemyUnit(this.enemyData.slot2);
                    break;
                case 6:
                    esm.Enemy = new EnemyUnit(this.enemyData.slot3);
                    break;
                case 7:
                    esm.Enemy = new EnemyUnit(this.enemyData.slot4);
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
        this.playerData.FullHeal();
    }

    private void HealEnemyParty()
    {
        this.enemyData.FullHeal();
    }
}
