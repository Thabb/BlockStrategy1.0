using UnityEngine;

namespace Health
{
    public abstract class HealthEntity : MonoBehaviour
    {
        public HealthBar healthBar;

        public float maxHealth;
        public float currentHealth;
        public float attackDamage;
        public float armor;
        public float range;
        public float attackSpeed;

        public int team;

        private bool isAlive = true;

        public void Start()
        {
            healthBar.SetHealth(this);
        }

        public void TakeDamage(float incomingDamage)
        {
            currentHealth -= (incomingDamage / armor) * 10;
            healthBar.SetHealth(this);

            if (currentHealth <= 0) DestroySelf();
        }

        private void DestroySelf()
        {
            if (isAlive) Destroy(gameObject);
            isAlive = false;
        }
    }
}
