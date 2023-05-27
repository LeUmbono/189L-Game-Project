using System.Collections;
using UnityEngine;

namespace Combat
{
    public class PlayerStateMachine : GenericUnitStateMachine
    {
        public PlayerUnit Player;

        void Start()
        {
            CurrentState = TurnState.WAIT;
            csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();
            uism.HealthBars[Location].GetComponent<HealthBar>().SetMaxHealth(Player.MaxHP);
            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Player.CurrentHP);
        }

        void Update()
        {
            switch (CurrentState)
            {
                case TurnState.WAIT:
                    break;
                case TurnState.SELECTACTION:
                    break;
                case TurnState.ATTACK:
                    StartCoroutine(PerformAttack());
                    break;
                case TurnState.SWAP:
                    StartCoroutine(PerformSwap());
                    break;
                case TurnState.SPECIAL:
                    StartCoroutine(PerformSpecial());
                    break;
                case TurnState.DEAD:
                    if (isDead)
                    {
                        return;
                    }
                    else
                    {
                        // Make dead character invulnerable to enemy attack.
                        csm.AlliesInBattle.Remove(this.gameObject);

                        // Change sprite to reflect death / play death animation.
                        this.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

                        // Move dead player to furthest end of the formation.
                        for (int i = location; i > 0; i--)
                        {
                            var targetToSwap = csm.UnitsInBattle[i - 1];
                            if (targetToSwap.GetComponent<PlayerStateMachine>().isDead == false)
                            {
                                DoSwap(targetToSwap);
                            }
                            else
                            {
                                break;
                            }
                        }

                        isDead = true;

                        // Remove unit from turn list.
                        csm.TurnOrder.Remove(this.gameObject);
                    }
                    break;
            }
        }

        protected override void DoDamage()
        {
            UnitToTarget.GetComponent<EnemyStateMachine>().TakeDamage(Player.Attack);
        }

        public override void TakeDamage(float damage)
        {
            var damageTaken = Mathf.Max(damage - Player.Defense, 1.0f);
            Player.CurrentHP -= damageTaken;

            if (Player.CurrentHP <= 0.0f)
            {
                Player.CurrentHP = 0.0f;
                CurrentState = TurnState.DEAD;
            }

            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Player.CurrentHP);
        }

        private IEnumerator PerformAttack()
        {
            if (actionStarted)
            {
                yield break;
            }

            actionStarted = true;

            // Animate player to attack enemy unit.
            var initialPosition = transform.position;
            var targetPosition = UnitToTarget.transform.position - new Vector3(1f, 0f, 0f);
            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            // Pause for 0.5 seconds.
            yield return new WaitForSeconds(0.5f);

            // Do damage.
            DoDamage();

            // Animate enemy back to initial position.
            while (MoveTowardsPosition(initialPosition))
            {
                yield return null;
            }

            // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            // Set combat state of CSM to CheckGame.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

            //csm.CurrentUIState = CombatStateMachine.UIStates.ACTIVATE;

            actionStarted = false;
            CurrentState = TurnState.WAIT;
        }

        private IEnumerator PerformSwap()
        {
            if (actionStarted)
            {
                yield break;
            }

            actionStarted = true;

            DoSwap(UnitToTarget);

            // Remove this enemy game object from front of turn queue
            // and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            // Set combat state of CSM to Wait.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.WAIT;
            uism.CurrentUIState = UIStateMachine.UIStates.ACTIVATE;

            actionStarted = false;
            CurrentState = TurnState.WAIT;
        }

        private IEnumerator PerformSpecial()
        {
            if (actionStarted)
            {
                yield break;
            }

            actionStarted = true;

            // Animation probably in execute later.
            var initialPosition = transform.position;
            var targetPosition = UnitToTarget.transform.position - new Vector3(1f, 0f, 0f);
            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            // Pause for 0.5 seconds.
            yield return new WaitForSeconds(0.5f);

            // See if this works.
            Player.BaseClassData.SpecialAbility.Execute(this.gameObject);

            // Animation probably in execute later.
            while (MoveTowardsPosition(initialPosition))
            {
                yield return null;
            }

            // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            // Set combat state of CSM to CheckGame.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

            actionStarted = false;
            CurrentState = TurnState.WAIT;
        }
        private bool MoveTowardsPosition(Vector3 target)
        {
            return target != (transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime));
        }


    }
}
