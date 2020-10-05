using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonChicken
{
    public delegate void EventHandler();
    class Health : Component
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; private set; }
        public event EventHandler OnDeath;
        
        public Health()
        {
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
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
            CheckDeath();
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

        private void CheckDeath()
        {
            if (CurrentHealth <= 0)
            {
                OnDeath();
            }
        }
    }
}
