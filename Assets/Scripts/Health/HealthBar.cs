using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBar;
        public TMP_Text healthBarText;

        private Quaternion lookRotation;

        private void Start()
        {
            lookRotation = Camera.main.transform.rotation;
        }

        private void Update()
        {
            transform.parent.rotation = lookRotation;
        }

        public void SetHealth(HealthEntity healthEntity)
        {
            healthBar.maxValue = healthEntity.maxHealth;
            healthBar.value = healthEntity.currentHealth;

            healthBarText.text = $"{healthBar.value:N0} / {healthBar.maxValue:N0}";
        }
    }
}
