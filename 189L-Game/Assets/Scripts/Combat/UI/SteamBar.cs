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
            Neutral,
            Overclocked
        }

        [SerializeField] private int inertThreshold = 40;
        [SerializeField] private int neutralThreshold = 60;

        public void SetSteam(float steam)
        {
            slider.value = steam;
        }

        public SteamValue GetSteamValue()
        {
            if (0 < slider.value && slider.value < inertThreshold)
            {
                return SteamValue.Inert;
            }
            else if (inertThreshold < slider.value && slider.value < neutralThreshold)
            {
                return SteamValue.Neutral;
            }
            else if (neutralThreshold < slider.value && slider.value < 100)
            {
                return SteamValue.Overclocked;
            }
            else
            {
                return SteamValue.Neutral;
            }
        }
    }
}
