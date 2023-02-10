using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;
using L4P.Gameplay.Enemy;

namespace L4P.Gameplay.Weapons
{
    public class RaycastRange : Weapon
    {
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;
        private void Awake()
        {
            owner = GetComponentInParent<Animator>().transform;
        }
        public override void Use(bool performed)
        {
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                startingPoint.rotation = owner.rotation;
                foreach (var prtcl in shotPrtcls) 
                    prtcl.Play();

                Debug.DrawLine(startingPoint.position, startingPoint.position + startingPoint.forward * stats.range, Color.red, 1f);

                if (Physics.Raycast(startingPoint.position, owner.forward, out var contactHit, stats.range, ennemyLayer))
                {
                    var victim = contactHit.collider.attachedRigidbody.GetComponent<Hitable>();
                    if (victim)
                        victim.TakeDamage(stats.damage);
                }
            }
        }
    }
}
