using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using Overworld;
using Unity.VisualScripting;

public class SceneGameManager : MonoBehaviour
{
    [SerializeField] private string combatSceneName;
    [SerializeField] private string overworldSceneName;
    [SerializeField] private string titleSceneName;
    [SerializeField] private float timeToWait;
    [SerializeField][Range(0, 1)] private float slowdownPercent;
    [SerializeField] private Material transitionMaterial;
    private const int UILayerInt = 5;
  

    // Holds reference to overworld player.
    private PlayerController overworldPlayer;
    private AudioSource encounterAudioSource;

    // Holds the most recent references to player and enemy parties.
    public PartyData playerData;
    public PartyData enemyData;

    private void Awake()
    {

        //overworldMusicPlayer = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        encounterAudioSource = this.GetComponent<AudioSource>();

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
                    this.playerData.slot1curHP = psm.Unit.CurrentHP;
                    break;
                case 1:
                    this.playerData.slot2curHP = psm.Unit.CurrentHP;
                    break;
                case 2:
                    this.playerData.slot3curHP = psm.Unit.CurrentHP;
                    break;
                case 3:
                    this.playerData.slot4curHP = psm.Unit.CurrentHP;
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
        SceneManager.LoadScene(titleSceneName, LoadSceneMode.Single);
        yield return null;
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

        overworldPlayer.EnableInput();

        SceneManager.UnloadSceneAsync(combatSceneName);

        HealPlayerParty();
        // Resume overworld theme.
        var musicPlayer = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        musicPlayer.Play();
    }

    public IEnumerator LoadCombatScene(PartyData pData, PartyData eData)
    {
        // Stop overworld theme when entering combat.
        var musicPlayer = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        musicPlayer.Stop();

        // Play encounter sound effect.
        encounterAudioSource.Play();

        while (PlayingTransition())
        {
            yield return null;
        }

        encounterAudioSource.Stop();

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
                    psm.Unit = new PlayerUnit(this.playerData.slot1, this.playerData.slot1name, this.playerData.slot1curHP);
                    break;
                case 1:
                    psm.Unit = new PlayerUnit(this.playerData.slot2, this.playerData.slot2name, this.playerData.slot2curHP);
                    break;
                case 2:
                    psm.Unit = new PlayerUnit(this.playerData.slot3, this.playerData.slot3name, this.playerData.slot3curHP);
                    break;
                case 3:
                    psm.Unit = new PlayerUnit(this.playerData.slot4, this.playerData.slot4name, this.playerData.slot4curHP);
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
                    esm.Unit = new EnemyUnit(this.enemyData.slot1);
                    break;
                case 5:
                    esm.Unit = new EnemyUnit(this.enemyData.slot2);
                    break;
                case 6:
                    esm.Unit = new EnemyUnit(this.enemyData.slot3);
                    break;
                case 7:
                    esm.Unit = new EnemyUnit(this.enemyData.slot4);
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
            if(obj.layer != UILayerInt)
            {
                obj.SetActive(value);
            }
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
        // Animate the cutoff shader over time.
        var cutoff = transitionMaterial.GetFloat("_Cutoff") + Time.deltaTime;
        transitionMaterial.SetFloat("_Cutoff", cutoff);
        
        // After 1 second, the transition is done.
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
