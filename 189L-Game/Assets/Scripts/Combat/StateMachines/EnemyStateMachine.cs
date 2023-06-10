using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat
{
    public class EnemyStateMachine : GenericUnitStateMachine
    {
        //public EnemyUnit Enemy;
        public bool IsTaunted;
        void Start()
        {
            // Instantiate class variables.
            isDead = false;
            IsTaunted = false;
            CurrentState = TurnState.WAIT;

            // Set sprite of player based on incoming party data.
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Unit.BaseClassData.ClassSprite;

            // Instantiate combat scene game object variables.
            audioSource = this.GetComponent<AudioSource>();
            csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();

            // Initialize health bar.
            uism.HealthBars[Location].GetComponent<HealthBar>().SetMaxHealth(Unit.MaxHP);
            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Unit.CurrentHP);
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
                    // The flag isDead ensures that death code is only executed once.
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

                        // Disable dead unit health bar.
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
                UnitToTarget = TargetPlayerUnit();
            }
            
            CurrentState = TurnState.ATTACK;
        }

        private GameObject TargetPlayerUnit()
        {
            // Select a target based on a biased targeting engine that favors player units in front.
            var randomSeed = Random.Range(0, 100);
            var playerCount = CombatStateMachine.AlliesInBattle.Count-1;

            GameObject targetUnit = null;

            if (0 <= randomSeed && randomSeed < EnemyTargetingEngine.TargetingProbabilities[playerCount][0])
            {
                targetUnit = CombatStateMachine.AlliesInBattle.Find(player => player.GetComponent<PlayerStateMachine>().Location == 3);
            }
            else if (EnemyTargetingEngine.TargetingProbabilities[playerCount][0] <= randomSeed 
                && randomSeed < EnemyTargetingEngine.TargetingProbabilities[playerCount][1])
            {
                targetUnit = CombatStateMachine.AlliesInBattle.Find(player => player.GetComponent<PlayerStateMachine>().Location == 2);
            }
            else if (EnemyTargetingEngine.TargetingProbabilities[playerCount][1] <= randomSeed 
                && randomSeed < EnemyTargetingEngine.TargetingProbabilities[playerCount][2])
            {
                targetUnit = CombatStateMachine.AlliesInBattle.Find(player => player.GetComponent<PlayerStateMachine>().Location == 1);
            }
            else
            {
                targetUnit = CombatStateMachine.AlliesInBattle.Find(player => player.GetComponent<PlayerStateMachine>().Location == 0);
            }

            return targetUnit;
        }

        protected override void DoDamage()
        {
            UnitToTarget.GetComponent<PlayerStateMachine>().TakeDamage(Unit.Attack);
        }

        public override void TakeDamage(float damage)
        {
            // Damage formula calculated by taking the incoming damage minus the enemy's defense. 
            // At least 1 point of damage is taken at any given time.

            var damageTaken = Mathf.Max(damage - Unit.Defense, 1.0f);
            Unit.CurrentHP -= damageTaken;

            // Enemy dies if current HP goes below 0.
            if (Unit.CurrentHP <= 0.0f)
            {
                Unit.CurrentHP = 0.0f;
                CurrentState = TurnState.DEAD;

                // Play death sound effect.
                if (deathSound != null)
                {
                    PlaySound(deathSound);
                }
            }
            else
            {
                // Play take damage sound effect.
                if (takeDamageSound != null)
                {
                    PlaySound(takeDamageSound);
                }
            }

            // Update health bar of enemy.
            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(Unit.CurrentHP);
        }

        private IEnumerator PerformAttack()
        {
            if (actionStarted)
            {
                yield break;
            }

            actionStarted = true;

            yield return new WaitForSeconds(0.1f);

            // Animate enemy to attack player unit.
            var initialPosition = transform.position;
            var targetPosition = new Vector3(UnitToTarget.GetComponent<SpriteRenderer>().bounds.max.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x,
                UnitToTarget.transform.position.y,
                UnitToTarget.transform.position.z);
            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            // Pause for 0.1 seconds.
            yield return new WaitForSeconds(0.1f);

            // Do damage.
            DoDamage();

            var player = UnitToTarget.GetComponent<PlayerStateMachine>();
            yield return new WaitWhile(() => player.IsPlaying());

            // Once taunted unit attacks provoker, reset taunted status.
            IsTaunted = false;

            // Flip sprite on way back.
            spriteRenderer.flipX = !spriteRenderer.flipX;

            // Animate enemy back to initial position.
            while (MoveTowardsPosition(initialPosition))
            {
                yield return null;
            }

            // Flip sprite to correct orientation once back to original position.
            spriteRenderer.flipX = true;

            // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            // Set combat state of CSM to CheckGame.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

            actionStarted = false;
            CurrentState = TurnState.WAIT;
        }

        private bool MoveTowardsPosition(Vector3 target)
        {
            System.Action<ITween<Vector3>> updatePos = (t) =>
            {
                transform.position = t.CurrentValue;
            };

            this.gameObject.Tween("Movement", transform.position, target, 0.2f, TweenScaleFunctions.SineEaseOut, updatePos);
            return target != transform.position;
        }
    }
}
