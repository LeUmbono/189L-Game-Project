using DigitalRuby.Tween;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Combat
{
    public class PlayerStateMachine : GenericUnitStateMachine
    {
        public PlayerUnit Player;
        public float BuffAmount;
        
        void Start()
        {
            isDead = false;
            BuffAmount = 0.0f;
            CurrentState = TurnState.WAIT;

            this.gameObject.GetComponent<SpriteRenderer>().sprite = Player.BaseClassData.ClassSprite;
            audioSource = this.GetComponent<AudioSource>();
            steamBar = GameObject.Find("SteamBar").GetComponent<SteamBar>();
            csm = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
            uism = GameObject.Find("UIManager").GetComponent<UIStateMachine>();
            uism.HealthBars[Location].GetComponent<HealthBar>().SetMaxHealth(Player.MaxHP);
            UpdateHealthBar(Player.CurrentHP);
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
                        CombatStateMachine.AlliesInBattle.Remove(this.gameObject);

                        // Change sprite to reflect death / play death animation.
                        this.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        uism.HealthBars[Location].SetActive(false);

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
                        CombatStateMachine.TurnOrder.Remove(this.gameObject);
                    }
                    break;
            }
        }

        protected override void DoDamage()
        {
            UnitToTarget.GetComponent<EnemyStateMachine>().TakeDamage(Player.Attack + BuffAmount);
        }

        public override void TakeDamage(float damage)
        {
            var damageTaken = Mathf.Max(damage - Player.Defense, 1.0f);
            Player.CurrentHP -= damageTaken;

            if (Player.CurrentHP <= 0.0f)
            {
                Player.CurrentHP = 0.0f;
                CurrentState = TurnState.DEAD;
                if(deathSound != null)
                {
                    PlaySound(deathSound);
                }
            }
            else
            {
                if(takeDamageSound != null)
                {
                    PlaySound(takeDamageSound);
                }
            }
            UpdateHealthBar(Player.CurrentHP);
        }

        public void UpdateHealthBar(float health)
        {
            uism.HealthBars[Location].GetComponent<HealthBar>().SetHealth(health);
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
            var targetPosition = new Vector3(UnitToTarget.GetComponent<SpriteRenderer>().bounds.min.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, 
                UnitToTarget.transform.position.y, 
                UnitToTarget.transform.position.z);

            yield return new WaitForSeconds(0.25f);

            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            // Pause for 0.5 seconds.
            yield return new WaitForSeconds(0.25f);

            // Do damage.
            DoDamage();

            var enemy = UnitToTarget.GetComponent<EnemyStateMachine>();
            yield return new WaitWhile(() => enemy.IsPlaying());

            // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            steamBar.ChangeSteam(10.0f);

            // Flip sprite on way back.
            this.gameObject.GetComponent<SpriteRenderer>().flipX = !this.gameObject.GetComponent<SpriteRenderer>().flipX;

            // Animate enemy back to initial position.
            while (MoveTowardsPosition(initialPosition))
            {
                yield return null;
            }

            // Flip sprite to correct orientation once back to original position.
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

            // Set combat state of CSM to CheckGame.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

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

            yield return new WaitForSeconds(0.25f);

            if (swapSound != null) 
            {
                PlaySound(swapSound);
            }
            
            DoSwap(UnitToTarget);
            
            // Heal 10% of max HP. 
            this.Player.CurrentHP += 0.1f * this.Player.MaxHP;
            if (this.Player.CurrentHP > this.Player.MaxHP)
            {
                this.Player.CurrentHP = this.Player.MaxHP;
            }

            // Remove this enemy game object from front of turn queue
            // and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            steamBar.ChangeSteam(-10.0f);

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

            yield return new WaitForSeconds(0.25f);

            // Animation probably in execute later.
            var initialPosition = transform.position;
            var targetPosition = new Vector3 (UnitToTarget.GetComponent<SpriteRenderer>().bounds.min.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, 
                UnitToTarget.transform.position.y, 
                UnitToTarget.transform.position.z);          
            
            while (MoveTowardsPosition(targetPosition))
            {
                yield return null;
            }

            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            
            // Pause for 0.25 seconds.
            yield return new WaitForSeconds(0.25f);

            // See if this works.
            Player.BaseClassData.SpecialAbility.Execute(this.gameObject);

            yield return new WaitWhile(() => IsPlaying());

            // Animation probably in execute later.
            while (MoveTowardsPosition(initialPosition))
            {
                yield return null;
            }

            // Flip sprite to correct orientation once back to original position.
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

            // Remove this enemy game object from front of turn queue and re-add back at the back of the queue.
            csm.EndTurn(this.gameObject);

            steamBar.ChangeSteam(Player.BaseClassData.SpecialAbility.GetSteamBarChangeValue());

            // Set combat state of CSM to CheckGame.
            csm.CurrentCombatState = CombatStateMachine.CombatStates.CHECKGAME;

            actionStarted = false;
            CurrentState = TurnState.WAIT;
        }
        private bool MoveTowardsPosition(Vector3 target)
        {
            var direction = target.x - transform.position.x;

            if(direction < 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            System.Action<ITween<Vector3>> updatePos = (t) =>
            {
                transform.position = t.CurrentValue;
            };

            this.gameObject.Tween("Movement", transform.position, target, 0.4f, TweenScaleFunctions.SineEaseOut, updatePos);
            return target != transform.position;
        }
    }
}
