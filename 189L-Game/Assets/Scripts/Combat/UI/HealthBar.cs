using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        private TextMeshProUGUI hpTextbox;

        void Start()
        {
            hpTextbox = this.gameObject.transform.Find("HPText").GetComponent<TextMeshProUGUI>();
        }

        public void SetMaxHealth(float health)
        {
            slider.maxValue = health;
            slider.value = health;
        }
        public void SetHealth(float health)
        {
            slider.value = health;
            hpTextbox.text = slider.value + " / " + slider.maxValue;
        }
    }
}
