using System.Collections;
using UnityEngine;
using L4P.Gameplay.Weapons;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        PlayerAnimatorController animator;
        PlayerAnimatorEvent animatorEvent;

        [SerializeField] private Weapon currentRightHand;
        public Weapon CurrentRightHand { get => currentRightHand; }

        [SerializeField] private Weapon currentLeftHand;
        public Weapon CurrentLeftHand { get => currentLeftHand; }

        private bool useLeftWeapon = false;
        private bool useRightWeapon = false;
        private bool isRightHand = true;

        private bool isAttacking;

        public void UseWeapon(bool performed, bool isRightHand)
        {
            if (isAttacking && performed) return;
            this.isRightHand = isRightHand;
            if (isRightHand) useRightWeapon = performed;
            else useLeftWeapon = performed;
            Weapon weapon = isRightHand ? currentRightHand : currentLeftHand;
            weapon.Use(performed, animator);
            //animator.SetAttackAnimationTrigger(performed);
        }
        private void Update()
        {
            if (useRightWeapon)
            {
                if (currentRightHand.NextHit <= Time.time)
                {
                    currentRightHand.Use(true, animator);
                }
            }
            if(useLeftWeapon)
            {
                if (!isRightHand && currentLeftHand.NextHit <= Time.time)
                {
                    currentLeftHand.Use(true, animator);
                }
            }
        }
        private void Awake()
        {
            //currentWeapon = GetComponentInChildren<IWeapon>();
            animator = GetComponent<PlayerAnimatorController>();
            animatorEvent = GetComponentInChildren<PlayerAnimatorEvent>();

            animatorEvent.IsAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsNotAttacking.AddListener(delegate { isAttacking = false; });

            //if (currentLeftHand is MeleeWeapon)
            animatorEvent.LeftActivateEvent.AddListener(delegate { ((MeleeWeapon)currentLeftHand).Trigger.IsActive = true; });
            animatorEvent.LeftDeactivateEvent.AddListener(delegate { ((MeleeWeapon)currentLeftHand).Trigger.IsActive = false; });
            animatorEvent.RightActivateEvent.AddListener(delegate { ((MeleeWeapon)currentRightHand).Trigger.IsActive = true; });
            animatorEvent.RightDeactivateEvent.AddListener(delegate { ((MeleeWeapon)currentRightHand).Trigger.IsActive = false; });
            animatorEvent.ResetComboEvent.AddListener(delegate { animator.SetCombo(false); });
        }
    }
}