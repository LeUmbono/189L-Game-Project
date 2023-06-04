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

        public static List<GameObject> AlliesInBattle = new List<GameObject>();
        public static List<GameObject> EnemiesInBattle = new List<GameObject>();

        // List that tracks the positions of the units in battle.
        public List<GameObject> UnitsInBattle = new List<GameObject>();

        // List that tracks the turn order queue.
        public static List<GameObject> TurnOrder = new List<GameObject>();

        [SerializeField] private GameObject targetIndicator;
        private UIStateMachine uism;

        private SceneGameManager gameManager;

        void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<SceneGameManager>();
            gameManager.InitializeCombatScene();
        }

        void Start()
        {
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
                    targetIndicator.transform.position = TurnOrder[0].transform.position
                        + new Vector3(0.0f, 1.0f, 0.0f);
                    targetIndicator.SetActive(true);

                    var GSM = TurnOrder[0].GetComponent<GenericUnitStateMachine>();
                    GSM.CurrentState = GenericUnitStateMachine.TurnState.SELECTACTION;

                    CurrentCombatState = CombatStates.PERFORMACTION;
                    break;
                case CombatStates.PERFORMACTION:
                    targetIndicator.transform.position = TurnOrder[0].transform.position
                        + new Vector3(0.0f, 1.0f, 0.0f);
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
            TurnOrder.RemoveAt(0);
            TurnOrder.Add(unit);
        }

    }
}
