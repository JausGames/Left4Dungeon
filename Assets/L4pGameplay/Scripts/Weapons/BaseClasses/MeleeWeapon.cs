using UnityEngine;
using L4P.Gameplay.Player.Animations;


namespace L4P.Gameplay.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] protected WeaponTrigger trigger;

        public WeaponTrigger Trigger { get => trigger; set => trigger = value; }

        private void Awake()
        {
            trigger = GetComponentInChildren<WeaponTrigger>();
        }
        public override void Use(bool performed, PlayerAnimatorController playerAnimator)
        {
            Debug.Log("MeleeWeapon, Use : " + performed);
            if (playerAnimator.Comboable && performed)
            {
                Debug.Log("MeleeWeapon, Use : SetCombo true");
                playerAnimator.SetCombo(performed);
            }

            else if (performed)
            {
                Debug.Log("MeleeWeapon, Use : SetAttackAnimationTrigger");
                playerAnimator.SetAttackAnimationTrigger(performed, stance == Stance.Left ? leftAnimTriggerHash : rightAnimTriggerHash);

                NextHit = Time.time + stats.cooldown;
            }
        }
    }
}
