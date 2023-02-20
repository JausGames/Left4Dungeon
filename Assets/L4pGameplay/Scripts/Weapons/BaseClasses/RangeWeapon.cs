using L4P.Gameplay.Player.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Weapons
{
    abstract public class RangeWeapon : Weapon
    {
        [SerializeField] protected LayerMask ennemyLayer;
        [SerializeField] protected ParticleSystem[] shotPrtcls;
        [SerializeField] protected Transform startingPoint;

        public override void Use(bool performed, PlayerAnimatorController playerAnimator)
        {
            //do it with hash
            playerAnimator.SetAttackAnimationBool(performed, stance == Stance.Right, stance == Stance.Left ? leftAnimTriggerHash : rightAnimTriggerHash);
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                PerformShoot();
            }

        }
        public override void UseWeak(bool performed, PlayerAnimatorController playerAnimator)
        {
            //do it with hash
            playerAnimator.SetAttackAnimationBool(performed, stance == Stance.Right, stance == Stance.Left ? leftAnimTriggerHash : rightAnimTriggerHash);
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                PerformShoot();
            }

        }
        public override void UseStrong(bool performed, PlayerAnimatorController playerAnimator)
        {
            //do it with hash
            playerAnimator.SetAttackAnimationBool(performed, stance == Stance.Right, stance == Stance.Left ? leftAnimTriggerHash : rightAnimTriggerHash);
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                PerformShoot();
            }

        }

        abstract protected void PerformShoot();
    }
}
