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
                trigger.ChangeStatsMultiplier(1f, 1f);
                Debug.Log("MeleeWeapon, Use : SetCombo true");
                playerAnimator.SetCombo(performed);
            }

            else if (!playerAnimator.GetHit && performed)
            {
                trigger.ChangeStatsMultiplier(1f, 1f);
                Debug.Log("MeleeWeapon, Use : SetAttackAnimationTrigger");
                playerAnimator.SetAttackAnimationTrigger(performed, rightAnimTriggerHash);

                NextHit = Time.time + stats.cooldown;
            }
        }
        public override void UseWeak(bool performed, PlayerAnimatorController playerAnimator)
        {
            Debug.Log("MeleeWeapon, Use : " + performed);
            if (playerAnimator.Comboable && performed)
            {
                trigger.ChangeStatsMultiplier(.5f, 1f);
                Debug.Log("MeleeWeapon, Use : SetCombo true");
                playerAnimator.SetCombo(performed);
            }

            else if (!playerAnimator.GetHit && performed)
            {
                Debug.Log("MeleeWeapon, Use : SetAttackAnimationTrigger");
                playerAnimator.SetAttackAnimationTrigger(performed, leftAnimTriggerHash);

                NextHit = Time.time + stats.cooldown;
            }
        }
        public override void UseStrong(bool performed, PlayerAnimatorController playerAnimator)
        {
            if (performed)
            {
                Debug.Log("MeleeWeapon, UseStrong : SetAttackAnimationTrigger");
                trigger.ChangeStatsMultiplier(3f, 2f);
                playerAnimator.SetAttackAnimationTrigger(performed, 304422123);

                NextHit = Time.time + stats.cooldown;
            }
        }
    }
}
