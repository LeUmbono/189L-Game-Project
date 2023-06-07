using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using Overworld;
using TMPro;

public class SceneGameManager : MonoBehaviour
{
    [SerializeField] private string combatSceneName;
    [SerializeField] private string overworldSceneName;
    [SerializeField] private string titleSceneName;
    [SerializeField] private float timeToWait;
    [SerializeField][Range(0, 1)] private float slowdownPercent;
    [SerializeField] private Material transitionMaterial;

    // Holds reference to overworld player.
    private PlayerController overworldPlayer;

    // Holds the most recent references to player and enemy parties.
    public PartyData playerData;
    public PartyData enemyData;

    private void Awake()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void SetOverworldPlayerVars(PlayerController player)
    {
        overworldPlayer = player;
        playerData = player.partyData;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        string sceneName = scene.name;

        if(string.Equals(sceneName, titleSceneName))
        {
            Debug.Log("Loaded Title Screen");
        }
        else if(string.Equals(sceneName, overworldSceneName))
        {
            Debug.Log("Loaded Overworld Scene");
            this.StartGame();
        }
        else if(string.Equals(sceneName, combatSceneName))
        {
            Debug.Log("Loaded Combat Scene");
            this.ToggleOverworld(false);
        }
        else
        {
            Debug.LogError("UNKNOWN SCENE LOADED");
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        string sceneName = scene.name;

        if(string.Equals(sceneName, titleSceneName))
        {
            //Debug.Log("Unloaded Title Screen");
        }
        else if(string.Equals(sceneName, overworldSceneName))
        {
            //Debug.Log("Unloaded Overworld Scene");
        }
        else if(string.Equals(sceneName, combatSceneName))
        {
            //Debug.Log("Unloaded Combat Scene");
            this.ToggleOverworld(true);
            transitionMaterial.SetFloat("_Cutoff", 0f);
        }
        else
        {
            Debug.LogError("UNKNOWN SCENE LOADED");
        }
    }

    // Only to be called from overworld / after combat.
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

    private void StartGame()
    {
        this.SetOverworldPlayerVars(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        this.HealPlayerParty();
    }

    public IEnumerator LoadTitleScene()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
        while (PlayingTransition())
        {
            yield return null;
        }

        SceneManager.LoadScene(titleSceneName, LoadSceneMode.Single);
    }
    
    public IEnumerator LoadOverworldScene()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
        while (PlayingTransition())
        {
            yield return null;
        }

        SceneManager.LoadScene(overworldSceneName, LoadSceneMode.Single);
        
    }

    public IEnumerator WinFunction()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
        while (PlayingTransition())
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(combatSceneName);
    }

    public IEnumerator LoadCombatScene(PartyData pData, PartyData eData)
    {
        while (PlayingTransition())
        {
            yield return null;
        }

        SceneManager.LoadScene(combatSceneName, LoadSceneMode.Additive);

        playerData = pData;
        enemyData = eData;
    }

    public void InitializeCombatScene()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Initializes from top to bottom based on scene hierarchy.
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

    private void ToggleOverworld(bool value)
    {
        GameObject[] objs = SceneManager.GetSceneByName(overworldSceneName).GetRootGameObjects();

        foreach(GameObject obj in objs)
        {
            obj.SetActive(value);
        }
    }

    private void HealPlayerParty()
    {
        this.playerData.FullHeal();
        Debug.Log("Healed Party");
    }

    private void HealEnemyParty()
    {
        this.enemyData.FullHeal();
    }

    private bool PlayingTransition()
    {
        var cutoff = transitionMaterial.GetFloat("_Cutoff") + Time.deltaTime;
        transitionMaterial.SetFloat("_Cutoff", cutoff);
        if (cutoff < 1.0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
