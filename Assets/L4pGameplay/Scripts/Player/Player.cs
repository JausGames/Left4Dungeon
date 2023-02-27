using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Player.Animations;
using L4P.Gameplay.Player.TopDown;
using L4P.Gameplay.Weapons;
using System;
using System.Linq;

namespace L4P.Gameplay.Player
{

    [RequireComponent(typeof(IPlayerCombat), typeof(IPlayerController), typeof(Controls.PlayerInputs))]
    public class Player : Hitable
    {
        PlayerAnimatorController animator;
        PlayerCombat combat;
        [SerializeField] HealthBar healthUi;

        [SerializeField] LayerMask interactableMask;
        [SerializeField] Interactable availableInteractable;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<PlayerAnimatorController>();
            combat = GetComponent<PlayerCombat>();
            healthUi.SetMaxHealth(maxHealth);
        }
        private void Update()
        {
            CheckInteractable();
        }

        private void CheckInteractable()
        {
            var cols = Physics.OverlapSphere(transform.position, 1f, interactableMask);

            if (cols.Length > 0)
            {
                var orderedByProximity = cols.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();
                availableInteractable = orderedByProximity[0].GetComponent<Interactable>();
            }
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

        internal void TryInteract()
        {
            if (availableInteractable)
                availableInteractable.Interact(this);
        }

        internal void TrySwitchWeapon(bool isRight)
        {
            if (availableInteractable && availableInteractable is WeaponHolder)
                if (isRight)
                {
                    var rightWeapon = combat.CurrentRightHand.Prefab;
                    var holder = ((WeaponHolder)availableInteractable);

                    combat.CurrentRightHand = holder.Prefab.GetComponent<Weapon>();
                    combat.CurrentRightHand.Owner = transform;
                    holder.Prefab = rightWeapon;
                }
                else
                {
                    var leftWeapon = combat.CurrentLeftHand.Prefab;
                    var holder = ((WeaponHolder)availableInteractable);

                    combat.CurrentLeftHand = holder.Prefab.GetComponent<Weapon>();
                    combat.CurrentLeftHand.Owner = transform;
                    holder.Prefab = leftWeapon;
                }
        }

        internal void RecoverHealth(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            healthUi.SetHealth(currentHealth);
        }
    }
}
