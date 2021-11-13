using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthBar : MonoBehaviour
    {
        /// <summary>
        /// Reference to the visual slider.
        /// </summary>
        public Slider healthBar;
        /// <summary>
        /// Reference to the text that displays the amount of health.
        /// </summary>
        public TMP_Text healthBarText;

        /// <summary>
        /// The rotation the healthbar gameobject should always maintain.
        /// </summary>
        private Quaternion lookRotation;

        /// <summary>
        /// Saves the rotation of the main camera for further use.
        /// </summary>
        private void Start()
        {
            lookRotation = Camera.main.transform.rotation;
        }

        /// <summary>
        /// Makes sure that the healthbar is always rotated towards the camera.
        /// </summary>
        private void Update()
        {
            transform.parent.rotation = lookRotation;
        }

        /// <summary>
        /// Changes the currently displayed health by scaling the healthbar visually as well as setting the correct numbers in text.
        /// </summary>
        /// <param name="healthEntity"></param>
        public void SetHealth(HealthEntity healthEntity)
        {
            healthBar.maxValue = healthEntity.maxHealth;
            healthBar.value = healthEntity.currentHealth;

            healthBarText.text = $"{healthBar.value:N0} / {healthBar.maxValue:N0}";
        }
    }
}
