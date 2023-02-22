using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Player.Animations;
using L4P.Gameplay.Player.TopDown;
using L4P.Gameplay.Weapons;

namespace L4P.Gameplay.Player
{

    [RequireComponent(typeof(IPlayerCombat), typeof(IPlayerController), typeof(Controls.PlayerInputs))]
    public class Player : Hitable
    {
        PlayerAnimatorController animator;
        [SerializeField] HealthBar healthUi;
        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<PlayerAnimatorController>();
            healthUi.SetMaxHealth(maxHealth);
        }
        public override void TakeDamage(WeaponStat stats, Vector3 direction)
        {
            Debug.Log("Take damage");
            base.TakeDamage(stats, direction);
            animator.GetHit = true;
            healthUi.SetHealth(currentHealth);
            var combat = GetComponent<PlayerCombat>();
            combat.DeactivateTriggers();
        }
        public override void Die()
        {
            base.Die();

            animator.Die();
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
