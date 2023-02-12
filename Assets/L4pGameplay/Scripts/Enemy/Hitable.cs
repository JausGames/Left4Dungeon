using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Enemy
{
    public class Hitable : MonoBehaviour
    {
        // Characteristics of the player
        protected float maxHealth = 100f;
        protected float currentHealth = 100f;

        protected bool alive;

        // Update player health
        virtual public void TakeDamage(float damage)
        {
            //if (currentHealth == 0f) return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        virtual public void Die()
        {
            // Code for death event
            Destroy(gameObject);
        }
    }
}
