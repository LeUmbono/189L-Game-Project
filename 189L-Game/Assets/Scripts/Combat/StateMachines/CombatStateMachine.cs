using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Combat
{
    public class CombatStateMachine : MonoBehaviour
    {
        // Combat state list.
        public enum CombatStates
        {
            WAIT,
            TAKEACTION,
            PERFORMACTION,
            CHECKGAME,
            WIN,
            LOSE,
        }

        public CombatStates CurrentCombatState;

        // Lists of player and enemy units in battle.
        public static List<GameObject> AlliesInBattle;
        public static List<GameObject> EnemiesInBattle;

        // List that tracks the positions of the units in battle.
        public List<GameObject> UnitsInBattle;

        // List that tracks the turn order queue.
        public static List<GameObject> TurnOrder;

        // UI information.
        [SerializeField] private GameObject targetIndicator;
        private UIStateMachine uism;

        private SceneGameManager gameManager;

        void Awake()
        {
            // Initializes combat scene with incoming player and enemy party data.
            gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
            gameManager.InitializeCombatScene();
        }

        void Start()
        {
            // Instantiate class variables.
            AlliesInBattle = new List<GameObject>();
            EnemiesInBattle = new List<GameObject>();
            UnitsInBattle = new List<GameObject>();
            TurnOrder = new List<GameObject>();

            // Get reference to UI State Machine.
            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();

            // Initialize allies and enemies lists.
            AlliesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
            EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            // Arrange unit positions.
            UnitsInBattle.AddRange(AlliesInBattle);
            UnitsInBattle.AddRange(EnemiesInBattle);
            ShuffleUnitPositions();

            // Arrange turn order.
            TurnOrder.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
            TurnOrder.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            ShuffleTurnOrder();

            CurrentCombatState = CombatStates.TAKEACTION;
        }

        void Update()
        {
            switch (CurrentCombatState)
            {
                case CombatStates.WAIT:
                    // Disable target indicator.
                    targetIndicator.SetActive(false);
                    // If turn order list is not empty, it's time for a unit to take action!
                    if (TurnOrder.Count > 0)
                    {
                        CurrentCombatState = CombatStates.TAKEACTION;
                    }
                    break;
                case CombatStates.TAKEACTION:
                    // Allows target indicator to follow unit.
                    targetIndicator.transform.position = TurnOrder[0].GetComponent<SpriteRenderer>().bounds.center + 
                        new Vector3(0.0f, TurnOrder[0].GetComponent<SpriteRenderer>().bounds.extents.y, 0.0f);
                    targetIndicator.SetActive(true);

                    // Set sprite to the top of the sorting order so that it display above all other sprites
                    // in the same layer.
                    var sprite = TurnOrder[0].GetComponent<SpriteRenderer>();
                    sprite.sortingOrder = 1;

                    var gsm = TurnOrder[0].GetComponent<GenericUnitStateMachine>();
                    gsm.CurrentState = GenericUnitStateMachine.TurnState.SELECTACTION;

                    CurrentCombatState = CombatStates.PERFORMACTION;
                    break;
                case CombatStates.PERFORMACTION:
                    // Allows target indicator to follow unit.
                    targetIndicator.transform.position = TurnOrder[0].GetComponent<SpriteRenderer>().bounds.center +
                        new Vector3(0.0f, TurnOrder[0].GetComponent<SpriteRenderer>().bounds.extents.y, 0.0f);
                    break;
                case CombatStates.CHECKGAME:
                    if (AlliesInBattle.Count < 1)
                    {
                        // Disable target indicator.
                        targetIndicator.SetActive(false);
                        uism.CurrentUIState = UIStateMachine.UIStates.WAIT;
                        CurrentCombatState = CombatStates.LOSE;
                    }
                    else if (EnemiesInBattle.Count < 1)
                    {
                        // Disable target indicator.
                        targetIndicator.SetActive(false);
                        uism.CurrentUIState = UIStateMachine.UIStates.WAIT;
                        CurrentCombatState = CombatStates.WIN;
                    }
                    else
                    {
                        uism.CurrentUIState = UIStateMachine.UIStates.ACTIVATE;
                        CurrentCombatState = CombatStates.WAIT;
                    }
                    break;
                case CombatStates.WIN:
                    Debug.Log("You win!");
                    gameManager.UpdatePlayerData();
                    StartCoroutine(gameManager.WinFunction());
                    break;
                case CombatStates.LOSE:
                    Debug.Log("You lose");
                    StartCoroutine(gameManager.LoadTitleScene());
                    break;
            }
        }

        public void ShuffleUnitPositions()
        {
            // Sort unit positions based on location.
            UnitsInBattle.Sort(delegate (GameObject x, GameObject y)
            {
                var xLocation = x.GetComponent<GenericUnitStateMachine>().Location;
                var yLocation = y.GetComponent<GenericUnitStateMachine>().Location;

                if (xLocation > yLocation)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
        }

        public static void ShuffleTurnOrder()
        {
            // Sort initial turn order based on agility of units.
            TurnOrder.Sort(delegate (GameObject x, GameObject y)
            {
                float xAgility, yAgility;

                xAgility = x.GetComponent<GenericUnitStateMachine>().Unit.Agility;
                yAgility = y.GetComponent<GenericUnitStateMachine>().Unit.Agility;

                if (xAgility < yAgility) return 1;
                else return -1;
            });
        }

        public void EndTurn(GameObject unit)
        {
            // Reduce order of sprite in scene.
            var sprite = TurnOrder[0].GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 0;
            
            // Remove unit at top of turn queue and add it at the end.
            TurnOrder.RemoveAt(0);
            TurnOrder.Add(unit);
        }

    }
}
