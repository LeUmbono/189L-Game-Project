using System;
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

        private static float overclockedThreshold = 40.0f;
        private static float shortcircuitedThreshold = 60.0f;

        [Range(0f, 100f)]
        private static float steamValue = 0.0f;
        private static SteamValue currentSteamState;

        void Start()
        {
            currentSteamState = SteamValue.Inert;
            slider.value = 0.0f;
        }

        void Update()
        {
            // Update the appearance of the steam bar.
            slider.value = steamValue;
        }

        public static void ChangeSteam(float steam)
        {
            steamValue += steam;

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

        private static void ApplySteamBarInertEffects()
        {
            // Reset all player unit stats to base stats.
            // Shuffle turn order.
            foreach(var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
            }

            CombatStateMachine.ShuffleTurnOrder();
        }

        private static void ApplySteamBarOverclockedEffects()
        {
            // Reset stats. Apply 1.5x multiplier to ATK, AGI.
            // Shuffle turn order.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.5f, 1.0f, 1.5f);
            }

            CombatStateMachine.ShuffleTurnOrder();
        }

        private static void ApplySteamBarShortcircuitedEffects()
        {
            // Reset stats. Apply 0.5x multiplier to DEF, AGI. 
            // Shuffle turn order.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.0f, 0.5f, 0.5f);
            }

            CombatStateMachine.ShuffleTurnOrder();
        }

        public static SteamValue GetSteamValue()
        {
            return currentSteamState;
        }
    }
}
