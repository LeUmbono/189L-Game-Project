using System;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Combat.GenericUnitStateMachine;

namespace Combat
{
    public class SteamBar : MonoBehaviour
    {

        public Slider slider;

        public enum SteamValue
        {
            Inert,
            InertDone,
            Overclocked,
            OverclockedDone,
            Shortcircuited,
            ShortcircuitedDone
        }

        [SerializeField] 
        private int overclockedThreshold = 40;
        [SerializeField] 
        private int shortcircuitedThreshold = 60;
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
            switch (currentSteamState)
            {
                case SteamValue.Inert:
                    ApplySteamBarInertEffects(); 
                    break;
                case SteamValue.InertDone:
                    // If steam bar crosses appropriate threshold, move to
                    // Overclocked state.
                    if (slider.value >= overclockedThreshold)
                    {
                        currentSteamState = SteamValue.Overclocked;
                    }
                    break;
                case SteamValue.Overclocked:
                    ApplySteamBarOverclockedEffects();
                    break;
                case SteamValue.OverclockedDone:
                    // If steam bar crosses shortcircuited threshold, move to
                    // Shortcircuited state. Else if steam bar falls below 
                    // overclocked threshold, move to Inert state.
                    if (slider.value >= shortcircuitedThreshold)
                    {
                        currentSteamState = SteamValue.Overclocked;
                    }
                    else if (slider.value < overclockedThreshold)
                    {
                        currentSteamState = SteamValue.Inert;
                    }
                    break;
                case SteamValue.Shortcircuited:
                    ApplySteamBarShortcircuitedEffects();
                    break;
                case SteamValue.ShortcircuitedDone:
                    // If steam bar falls below shortcircuited threshold, move to
                    // Overclocked state.
                    if (slider.value < shortcircuitedThreshold)
                    {
                        currentSteamState = SteamValue.Overclocked;
                    }
                    break;
            }

            slider.value = steamValue;
        }

        private void ApplySteamBarInertEffects()
        {
            // Reset all player unit stats to base stats.
            foreach(var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
            }

            currentSteamState = SteamValue.InertDone;
        }

        private void ApplySteamBarOverclockedEffects()
        {
            // Apply 1.5x multiplier to ATK, AGI. Shuffle turn order.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.5f, 1.0f, 1.5f);
            }

            currentSteamState = SteamValue.OverclockedDone;
        }

        private void ApplySteamBarShortcircuitedEffects()
        {
            // Reset stats. Apply 0.5x multiplier to DEF. 
            // Apply 1.5x multiplier to ATK, AGI. Shuffle turn order.
            foreach (var ally in CombatStateMachine.AlliesInBattle)
            {
                ally.GetComponent<PlayerStateMachine>().Player.ResetStats();
                ally.GetComponent<PlayerStateMachine>().Player.
                    ApplyMultiplierToStats(1.0f, 0.5f, 0.5f);
            }

            currentSteamState = SteamValue.ShortcircuitedDone;
        }

        public static void ChangeSteam(float steam)
        {
            steamValue += steam;
        }

        public static SteamValue GetSteamValue()
        {
            return currentSteamState;
        }
    }
}
