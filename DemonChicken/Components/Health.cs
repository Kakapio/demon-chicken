using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletTower
{
    class Health : Component
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public Health()
        {
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
        }
        public Health(int maxHealth, int currentHealth)
        {
            MaxHealth = MaxHealth;
            CurrentHealth = currentHealth;
        }

        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        /// <summary>
        /// Damage the entity by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void Damage(int amount)
        {
            CurrentHealth -= amount;

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }

        /// <summary>
        /// Heal the given entity by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void Heal(int amount)
        {
            CurrentHealth += amount;

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }
}
