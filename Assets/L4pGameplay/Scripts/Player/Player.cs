using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Player
{

    [RequireComponent(typeof(IPlayerCombat), typeof(IPlayerController), typeof(Controls.PlayerInputs))]
    public class Player : MonoBehaviour
    {
        // Characteristics of the player
        private float maxHealth = 100f;
        private float currentHealth = 100f;

        // Update player health
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // Code for death event
        }
    }
}
