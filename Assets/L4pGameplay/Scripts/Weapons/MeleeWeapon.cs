using UnityEngine;
using L4P.Gameplay.Player.Animations;


namespace L4P.Gameplay.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] protected int animTriggerHash;
        [SerializeField] protected WeaponTrigger trigger;

        public WeaponTrigger Trigger { get => trigger; set => trigger = value; }

        private void Awake()
        {
            trigger = GetComponentInChildren<WeaponTrigger>();
        }
        public override void Use(bool performed, PlayerAnimatorController playerAnimator)
        {
            if (performed && NextHit <= Time.time)
            {
                playerAnimator.SetAttackAnimationTrigger(performed, animTriggerHash);
                NextHit = Time.time + stats.cooldown;
            }
        }
    }
}
