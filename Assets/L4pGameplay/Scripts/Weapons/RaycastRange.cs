using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;
using L4P.Gameplay.Enemy;

namespace L4P.Gameplay.Weapons
{
    public class RaycastRange : MonoBehaviour, IWeapon
    {
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] WeaponStat stats = null;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;
        public WeaponStat Stats { get; }

        public void Use(bool performed)
        {
            if(performed)
            {
                foreach(var prtcl in shotPrtcls) 
                    prtcl.Play();

                Debug.DrawLine(startingPoint.position, startingPoint.position + startingPoint.forward * stats.range, Color.red, 1f);

                if (Physics.Raycast(startingPoint.position, startingPoint.forward, out var contactHit, stats.range, ennemyLayer))
                {
                    var victim = contactHit.collider.attachedRigidbody.GetComponent<Hitable>();
                    if (victim)
                        victim.TakeDamage(stats.damage);
                }
            }
        }
    }
}
