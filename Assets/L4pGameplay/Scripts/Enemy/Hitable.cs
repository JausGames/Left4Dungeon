using L4P.Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Enemy
{
    public class Hitable : MonoBehaviour
    {
        [Header("Status")]
        [SerializeField] protected bool alive;

        [Header("Characteristics")]
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth = 100f;

        protected Rigidbody body;

        public Rigidbody Body { get => body; }

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
            if(stats.knockback > 0f)
            {
                body.velocity = stats.knockback * direction;
                //GetComponent<Rigidbody>().AddForce(stats.knockback * direction, ForceMode.Impulse);
            }
        }

        virtual public void Die()
        {
            // Code for death event
            alive = false;
            body.velocity = Vector3.zero;
            body.isKinematic = true;
            body.useGravity = false;
        }
    }
}
