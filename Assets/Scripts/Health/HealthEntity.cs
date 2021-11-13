using UnityEngine;

namespace Health
{
    public abstract class HealthEntity : MonoBehaviour
    {
        /// <summary>
        /// The healthbar script, attched to every HealthEntity to show the health of said entity.
        /// </summary>
        public HealthBar healthBar;

        /// <summary>
        /// Part of the stat block. The maximum health the entity can have.
        /// </summary>
        public float maxHealth;
        /// <summary>
        /// Part of the stat block. The current health the entity has.
        /// </summary>
        public float currentHealth;
        /// <summary>
        /// Part of the stat block. The amount of damage the enitity deals of it attacks another entity.
        /// </summary>
        public float attackDamage;
        /// <summary>
        /// Part of the stat block. Armor is used to calculate the amount actual damage taken. For more look at TakeDamage().
        /// </summary>
        public float armor;
        /// <summary>
        /// Part of the stat block. This is the distance the entity needs to maintain to an attacked entity to keep attacking.
        /// </summary>
        public float range;
        /// <summary>
        /// Part of the stat block. The entity can attack as fast as [attackSpeed] per second.
        /// </summary>
        public float attackSpeed;

        /// <summary>
        /// The team the entity belongs to.
        /// </summary>
        public int team;

        /// <summary>
        /// Is the unit even alive?
        /// </summary>
        private bool isAlive = true;

        /// <summary>
        /// Initializes the healthbar, so it shows the correct numbers at the start of the game and not just after receiving damage for the first time.
        /// </summary>
        public void Start()
        {
            healthBar.SetHealth(this);
        }

        /// <summary>
        /// Does the damage calculation for incoming Damage and also applies this to the healthbar.
        /// </summary>
        /// <remarks>The damage calculation takes the attackers attack damage and the armor of the defender into account.
        /// The ratio of those two, towards each other is multiplied by the fixed amount of 10 and results in the damage,
        /// that actually comes through.
        /// If the attackers attack damage is higher than the defenders armor, it deals more then 10 damage. Also the other way around.</remarks>
        /// <param name="incomingDamage">The amount of attack damage the attacker deals to the defender.</param>
        public void TakeDamage(float incomingDamage)
        {
            currentHealth -= (incomingDamage / armor) * 10;
            healthBar.SetHealth(this);

            if (currentHealth <= 0) DestroySelf();
        }

        /// <summary>
        /// Destroy the HealthEntity.
        /// </summary>
        private void DestroySelf()
        {
            if (isAlive) Destroy(gameObject);
            isAlive = false;
        }
    }
}
