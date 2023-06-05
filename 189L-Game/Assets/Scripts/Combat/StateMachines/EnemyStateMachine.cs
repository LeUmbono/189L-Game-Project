using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat
{
    public class EnemyStateMachine : GenericUnitStateMachine
    {
        public EnemyUnit Enemy;
        public bool IsTaunted;
        void Start()
        {
            isDead = false;
            IsTaunted = false;
            CurrentState = TurnState.WAIT;

            audioSource = this.GetComponent<AudioSource>();
            csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();
            uism.HealthBars[Location].GetComponent<HealthBar>().SetMaxHealth(Enemy.MaxHP);
            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Enemy.CurrentHP);
        }

        void Update()
        {
            switch (CurrentState)
            {
                case TurnState.WAIT:
                    break;
                case TurnState.SELECTACTION:
                    SelectAction();
                    break;
                case TurnState.ATTACK:
                    StartCoroutine(PerformAttack());
                    break;
                case TurnState.DEAD:
                    if (isDead)
                    {
                        return;
                    }
                    else
                    {
                        // Make dead character invulnerable to enemy attack.
                        CombatStateMachine.EnemiesInBattle.Remove(this.gameObject);

                        // Change sprite to reflect death / play death animation.
                        this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                        uism.HealthBars[Location].SetActive(false);

                        // Move dead enemy to the furthest end of the formation.
                        for (int i = location; i < csm.UnitsInBattle.Count - 1; i++)
                        {
                            var targetToSwap = csm.UnitsInBattle[i + 1];
                            if (targetToSwap.GetComponent<EnemyStateMachine>().isDead == false)
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
                        CombatStateMachine.TurnOrder.Remove(this.gameObject);
                    }
                    break;
            }
        }

        private void SelectAction()
        {
            // Randomly select a player unit to target. If taunted, UnitToTarget has already
            // been set.

            // Check that taunted unit has died. If so, set IsTaunted to false.
            if(IsTaunted)
            {
                if(UnitToTarget.GetComponent<PlayerStateMachine>().IsDead == true)
                {
                    IsTaunted = false;
                }
            }

            if(!IsTaunted)
            {
                UnitToTarget = CombatStateMachine.AlliesInBattle[Random.Range(0, CombatStateMachine.AlliesInBattle.Count)];
            }
            
            CurrentState = TurnState.ATTACK;
        }

        protected override void DoDamage()
        {
            UnitToTarget.GetComponent<PlayerStateMachine>().TakeDamage(Enemy.Attack);
        }

        public override void TakeDamage(float damage)
        {
            var damageTaken = Mathf.Max(damage - Enemy.Defense, 1.0f);
            Enemy.CurrentHP -= damageTaken;

            if (Enemy.CurrentHP <= 0.0f)
            {
                Enemy.CurrentHP = 0.0f;
                CurrentState = TurnState.DEAD;
                if (deathSound != null)
                {
                    PlaySound(deathSound);
                }
            }
            else
            {
                if (takeDamageSound != null)
                {
                    PlaySound(takeDamageSound);
                }
            }

            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Enemy.CurrentHP);
        }

        private IEnumerator PerformAttack()
        {
            if (actionStarted)
            {
                yield break;
            }

            actionStarted = true;

            yield return new WaitForSeconds(0.5f);

            // Animate enemy to attack player unit.
            var initialPosition = transform.position;
            var targetPosition = UnitToTarget.transform.position + new Vector3(1f, 0f, 0f);
            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            // Pause for 0.5 seconds.
            yield return new WaitForSeconds(0.5f);

            // Do damage.
            DoDamage();

            // Once taunted unit attacks provoker, reset taunted status.
            IsTaunted = false;

            // Animate enemy back to initial position.
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
