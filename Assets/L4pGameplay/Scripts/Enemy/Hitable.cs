using L4P.Gameplay.Weapons;
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
        virtual public void TakeDamage(WeaponStat stats, Vector3 direction)
        {
            if (currentHealth == 0f) return;
            currentHealth -= stats.damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            GetComponent<Rigidbody>().AddForce(stats.knockback * direction, ForceMode.Impulse);
        }

        virtual public void Die()
        {
            // Code for death event
            Destroy(gameObject);
        }
    }
}
