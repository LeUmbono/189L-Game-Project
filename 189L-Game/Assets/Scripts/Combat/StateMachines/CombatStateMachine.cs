using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatStateMachine : MonoBehaviour
    {
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

        public static List<GameObject> AlliesInBattle;
        public static List<GameObject> EnemiesInBattle;

        // List that tracks the positions of the units in battle.
        public List<GameObject> UnitsInBattle;

        // List that tracks the turn order queue.
        public static List<GameObject> TurnOrder;

        [SerializeField] private GameObject targetIndicator;
        private UIStateMachine uism;

        private SceneGameManager gameManager;

        void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
            gameManager.InitializeCombatScene();
        }

        void Start()
        {
            AlliesInBattle = new List<GameObject>();
            EnemiesInBattle = new List<GameObject>();
            UnitsInBattle = new List<GameObject>();
            TurnOrder = new List<GameObject>();

            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();

            AlliesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
            EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            UnitsInBattle.AddRange(AlliesInBattle);
            UnitsInBattle.AddRange(EnemiesInBattle);
            ShuffleUnitPositions();

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
                    targetIndicator.SetActive(false);
                    if (TurnOrder.Count > 0)
                    {
                        CurrentCombatState = CombatStates.TAKEACTION;
                    }
                    break;
                case CombatStates.TAKEACTION:
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
                    StartCoroutine(gameManager.LoadOverworldScene());
                    break;
                case CombatStates.LOSE:
                    Debug.Log("You lose");
                    StartCoroutine(gameManager.LoadTitleScene());
                    break;
            }
        }

        public void ShuffleUnitPositions()
        {
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
            TurnOrder.Sort(delegate (GameObject x, GameObject y)
            {
                float xAgility, yAgility;

                if (x.tag == "Ally")
                {
                    xAgility = x.GetComponent<PlayerStateMachine>().Player.Agility;
                }
                else
                {
                    xAgility = x.GetComponent<EnemyStateMachine>().Enemy.Agility;
                }

                if (y.tag == "Ally")
                {
                    yAgility = y.GetComponent<PlayerStateMachine>().Player.Agility;
                }
                else
                {
                    yAgility = y.GetComponent<EnemyStateMachine>().Enemy.Agility;
                }

                if (xAgility < yAgility) return 1;
                else return -1;
            });
        }
        public void EndTurn(GameObject unit)
        {
            var sprite = TurnOrder[0].GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 0;
            TurnOrder.RemoveAt(0);
            TurnOrder.Add(unit);
        }

    }
}
