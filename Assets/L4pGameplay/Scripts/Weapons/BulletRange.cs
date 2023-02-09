using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;


namespace L4P.Gameplay.Weapons
{
    public class BulletRange : MonoBehaviour, IWeapon
    {
        [SerializeField] Transform owner;
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] WeaponStat stats = null;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;
        [SerializeField] GameObject bulletPrefab;
        public WeaponStat Stats { get => stats;}
        
        float nextHit = 0f;
        public float NextHit { get => nextHit; }

        private void Awake()
        {
            owner = GetComponentInParent<Animator>().transform;
        }
        public void Use(bool performed)
        {
            if (performed && nextHit <= Time.time)
            {
                nextHit = Time.time + stats.cooldown;
                foreach (var prtcl in shotPrtcls)
                    prtcl.Play();

                var bulletObj = Instantiate(bulletPrefab, startingPoint.position, owner.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetUp(stats.damage, stats.range, ennemyLayer);
            }
        }
    }
}
