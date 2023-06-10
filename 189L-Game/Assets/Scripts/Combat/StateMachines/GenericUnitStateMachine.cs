using UnityEngine;

namespace Combat
{
    public abstract class GenericUnitStateMachine : MonoBehaviour
    {
        // Unit state list.
        public enum TurnState
        {
            WAIT,
            SELECTACTION,
            SELECTTARGET,
            ATTACK,
            SWAP,
            SPECIAL,
            DEAD
        }

        // Run-time information about the unit.
        public TurnState CurrentState;
        public GameObject UnitToTarget;

        [SerializeField]
        protected int location;
        protected CombatStateMachine csm;
        protected UIStateMachine uism;
        protected SpriteRenderer spriteRenderer;
        protected bool actionStarted = false;
        protected bool isDead;

        // Combat scene information.
        protected AudioSource audioSource;
        protected SteamBar steamBar;

        // Sound clips.
        [SerializeField]
        protected AudioClip takeDamageSound;
        [SerializeField]
        protected AudioClip deathSound;
        [SerializeField]
        protected AudioClip swapSound;

        // Getters.
        public int Location => location;
        public bool IsDead => isDead;

        public void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public abstract void TakeDamage(float damage);

        protected abstract void DoDamage();

        protected void DoSwap(GameObject target)
        {
            // Switch positions of player unit and swapped target.
            var initialPosition = transform.position;

            this.gameObject.transform.position = target.transform.position;
            target.transform.position = initialPosition;

            // Change name of this variable.
            var targetLocation = target.GetComponent<GenericUnitStateMachine>().location;

            // Switch prefabs of associated buttons.
            GameObject thisButtonPrefab = uism.TargetButtons[location].
                GetComponent<TargetSelectButton>().TargetPrefab;

            uism.TargetButtons[location].GetComponent<TargetSelectButton>().
                TargetPrefab = uism.TargetButtons[targetLocation].
                GetComponent<TargetSelectButton>().TargetPrefab;

            uism.TargetButtons[targetLocation].GetComponent<TargetSelectButton>().
                TargetPrefab = thisButtonPrefab;

            // Switch positions of health bars.
            GameObject thisHealthBar = uism.HealthBars[location];
            var thisHealthBarPosition = thisHealthBar.transform.position;
            
            uism.HealthBars[location].transform.position = uism.HealthBars[targetLocation].transform.position;
            uism.HealthBars[targetLocation].transform.position = thisHealthBarPosition;

            uism.HealthBars[location] = uism.HealthBars[targetLocation];
            uism.HealthBars[targetLocation] = thisHealthBar;

            // Switch locations of player unit and swapped target
            var positionInBattle = csm.UnitsInBattle[location];
            csm.UnitsInBattle[location] = csm.UnitsInBattle[targetLocation];
            csm.UnitsInBattle[targetLocation] = positionInBattle;

            target.GetComponent<GenericUnitStateMachine>().location = location;
            location = targetLocation;
        }
    }
}
