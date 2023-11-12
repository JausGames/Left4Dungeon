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
        [SerializeField] private Transform rightHand;
        public Weapon CurrentRightHand 
        { 
            get => currentRightHand;
            set
            {
                var oldWeapon = currentRightHand;
                var newWeapon = ((GameObject)Instantiate(value.Prefab, rightHand)).GetComponent<Weapon>();
                currentRightHand = newWeapon;
                Destroy(oldWeapon.gameObject);
            }
        }

        [SerializeField] private Weapon currentLeftHand;
        [SerializeField] private Transform leftHand;
        public Weapon CurrentLeftHand
        {
            get => currentLeftHand;
            set
            {
                var oldWeapon = currentLeftHand;
                var newWeapon = ((GameObject)Instantiate(value.Prefab, leftHand)).GetComponent<Weapon>();
                currentLeftHand = newWeapon;
                Destroy(oldWeapon.gameObject);
            }
        }

        [SerializeField] private bool useLeftWeapon = false;
        [SerializeField] private bool useRightWeapon = false;
        [SerializeField] private AttackType currentRightAttack = AttackType.Right;

        [SerializeField] private bool isAttacking;

        public void UseWeapon(bool performed, AttackType type)
        {
            if (animator.Comboable && performed) animator.SetCombo(true);
            else if (!isAttacking || !performed)
            {
                var isRightHand = type == AttackType.Left ? false : true;

                if (isRightHand) useRightWeapon = performed;
                else useLeftWeapon = performed;

                if (isRightHand) currentRightAttack = type;

                //Weapon weapon = isRightHand ? currentRightHand : currentLeftHand;
                //weapon.Use(performed, animator);

            }
        }

        internal void DeactivateTriggers()
        {
            Debug.Log("DeactivateTriggers");
            if (CurrentRightHand is MeleeWeapon rightWeapon)
                rightWeapon.Trigger.IsActive = false;

            if (CurrentLeftHand is MeleeWeapon leftWeapon)
                leftWeapon.Trigger.IsActive = false;
        }

        private void Update()
        {
            if (animator.GetHit) { DeactivateTriggers(); return; } ;
            if (useRightWeapon)
            {
                if (currentRightHand.NextHit <= Time.time)
                {
                    switch (currentRightAttack)
                    {
                        case AttackType.Right:
                            currentRightHand.Use(true, animator);
                            break;
                        case AttackType.Left:
                            break;
                        case AttackType.Strong:
                            currentRightHand.UseStrong(true, animator);
                            break;
                        case AttackType.Alt:
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                currentRightHand.Use(false, animator);
                currentRightHand.UseStrong(false, animator);
            }
            if(useLeftWeapon)
            {
                if (currentLeftHand.NextHit <= Time.time)
                {
                    currentLeftHand.UseWeak(true, animator);
                }
            }
            else
            {
                currentLeftHand.UseWeak(false, animator);
            }
        }
        private void Awake()
        {
            //currentWeapon = GetComponentInChildren<IWeapon>();
            animator = GetComponent<PlayerAnimatorController>();
            animatorEvent = GetComponentInChildren<PlayerAnimatorEvent>();

            animatorEvent.IsAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsMobileAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsNotAttacking.AddListener(delegate { isAttacking = false; });
            animatorEvent.IsMobileNotAttacking.AddListener(delegate { isAttacking = false; });

            //if (currentLeftHand is MeleeWeapon)
            animatorEvent.LeftActivateEvent.AddListener(delegate { if (animator.GetHit) return; ((MeleeWeapon)currentLeftHand).Trigger.IsActive = true; });
            animatorEvent.LeftDeactivateEvent.AddListener(delegate { if (animator.GetHit) return; ((MeleeWeapon)currentLeftHand).Trigger.IsActive = false; });
            animatorEvent.RightActivateEvent.AddListener(delegate { if (animator.GetHit) return; ((MeleeWeapon)currentRightHand).Trigger.IsActive = true; });
            animatorEvent.RightDeactivateEvent.AddListener(delegate { if (animator.GetHit) return; ((MeleeWeapon)currentRightHand).Trigger.IsActive = false; });
            animatorEvent.ResetComboEvent.AddListener(delegate { animator.SetCombo(false); });
        }
    }
    public enum AttackType {
        Right,
        Left,
        Strong,
        Alt
    }
}