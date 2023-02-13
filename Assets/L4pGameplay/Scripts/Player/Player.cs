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
            base.TakeDamage(stats, direction);
            animator.GetHit();
            healthUi.SetHealth(currentHealth);
        }
        public override void Die()
        {
            alive = false;

            animator.Die();
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<Collider>().enabled = false;
            body.useGravity = false;

        }
    }
}
