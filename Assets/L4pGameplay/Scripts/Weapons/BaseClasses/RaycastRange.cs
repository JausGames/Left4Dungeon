using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Weapons
{
    public class RaycastRange : RangeWeapon
    {
        protected override void PerformShoot()
        {
            startingPoint.rotation = Owner.rotation;
            foreach (var prtcl in shotPrtcls)
                prtcl.Play();

            Debug.DrawLine(startingPoint.position, startingPoint.position + startingPoint.forward * stats.range, Color.red, 1f);

            if (Physics.Raycast(startingPoint.position, Owner.forward, out var contactHit, stats.range, ennemyLayer))
            {
                var victim = contactHit.collider.attachedRigidbody.GetComponent<Hitable>();
                if (victim)
                    victim.TakeDamage(stats, (victim.transform.position - owner.transform.position).normalized);
            }
        }
    }
}
