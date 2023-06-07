using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class SteamBar : MonoBehaviour
    {
        public Slider slider;

        public enum SteamValue
        {
            Inert,
            Overclocked,
            Shortcircuited
        }

        // Variables for combat music that changes with the steam bar.
        private static AudioSource audioSource;
        [SerializeField]
        private AudioClip inertTheme;
        [SerializeField]
        private AudioClip inertTransition;
        [SerializeField]
        private AudioClip overclockedTheme;
        [SerializeField]
        private AudioClip overclockedTransition;
        [SerializeField]
        private AudioClip shortcircuitedTheme;
        [SerializeField]
        private AudioClip shortcircuitedTransition;

        [SerializeField]
        private float overclockedThreshold = 40.0f;
        [SerializeField]
        private float shortcircuitedThreshold = 60.0f;
        private float steamValue = 0.0f;
        private SteamValue currentSteamState;

        void Start()
        {
            audioSource = GameObject.Find("CombatMusicManager").GetComponent<AudioSource>();
            audioSource.clip = inertTheme;
            currentSteamState = SteamValue.Inert;
            slider.value = 0.0f;

            audioSource.loop = true;
            audioSource.Play();
        }

        void Update()
        {
            // Update the appearance of the steam bar.
            slider.value = steamValue;
        }

        public void ChangeSteam(float steam)
        {
            steamValue += steam;

            // Ensure steam value remains within boundaries.
            if(steamValue < 0.0f) 
            { 
                steamValue = 0.0f;
            }
            else if(steamValue > 100.0f) 
            {
                steamValue = 100.0f;
            }

            var previousSteamState = currentSteamState;

            // Set the current steam state.
            if (0.0f <= steamValue && steamValue < overclockedThreshold)
            {
                currentSteamState = SteamValue.Inert;
            }
            else if (overclockedThreshold <= steamValue && steamValue < shortcircuitedThreshold)
            {
                currentSteamState = SteamValue.Overclocked;
            }
            else if (shortcircuitedThreshold <= steamValue && steamValue < 100.0f)
            {
                currentSteamState = SteamValue.Shortcircuited;
            }

            var isInDifferentState = previousSteamState != currentSteamState ? true : false;

            if (isInDifferentState)
            {
                switch (currentSteamState)
                {
                    case SteamValue.Inert:
                        ApplySteamBarInertEffects();
                        break;
                    case SteamValue.Overclocked:
                        ApplySteamBarOverclockedEffects();
                        break;
                    case SteamValue.Shortcircuited:
                        ApplySteamBarShortcircuitedEffects();
                        break;
                }
            }
        }

        private void ApplySteamBarInertEffects()
        {
            // Transition to inert theme.
            StartCoroutine(PlayInertTheme());
            // Reset all player unit stats to base stats.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
            }
        }

        private void ApplySteamBarOverclockedEffects()
        {
            // Transition to overclocked theme.
            StartCoroutine(PlayOverclockedTheme());
            // Reset stats. Apply 1.5x multiplier to ATK, AGI.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.5f, 1.0f, 1.5f);
            }
        }

        private void ApplySteamBarShortcircuitedEffects()
        {
            // Transition to shortcircuited theme.
            StartCoroutine(PlayShortcircuitedTheme());
            // Reset stats. Apply 0.5x multiplier to DEF, AGI.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.0f, 0.5f, 0.5f);
            }
        }

        public SteamValue GetSteamValue()
        {
            return currentSteamState;
        }

        IEnumerator PlayInertTheme()
        {
            /*
            audioSource.loop = false;
            audioSource.clip = inertTransition;
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
            */
            yield return new WaitForSeconds(1);

            audioSource.loop = true;
            audioSource.clip = inertTheme;
            audioSource.Play();
        }

        IEnumerator PlayOverclockedTheme()
        {
            audioSource.loop = false;
            audioSource.clip = overclockedTransition;
            audioSource.Play();

            yield return new WaitWhile(()=>audioSource.isPlaying);

            audioSource.loop = true;
            audioSource.clip = overclockedTheme;
            audioSource.Play();
        }

        IEnumerator PlayShortcircuitedTheme()
        {
            audioSource.loop = false;
            audioSource.clip = shortcircuitedTransition;
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);

            audioSource.loop = true;
            audioSource.clip = shortcircuitedTheme;
            audioSource.Play();
        }
    }
}
