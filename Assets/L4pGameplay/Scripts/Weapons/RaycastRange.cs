using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Weapons
{
    public class RaycastRange : Weapon
    {
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;

        public override void Use(bool performed, PlayerAnimatorController playerAnimator)
        {
            //do it with hash
            playerAnimator.SetAttackAnimation(performed);
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                startingPoint.rotation = Owner.rotation;
                foreach (var prtcl in shotPrtcls) 
                    prtcl.Play();

                Debug.DrawLine(startingPoint.position, startingPoint.position + startingPoint.forward * stats.range, Color.red, 1f);

                if (Physics.Raycast(startingPoint.position, Owner.forward, out var contactHit, stats.range, ennemyLayer))
                {
                    var victim = contactHit.collider.attachedRigidbody.GetComponent<Hitable>();
                    if (victim)
                        victim.TakeDamage(stats.damage);
                }
            }
        }
    }
}
