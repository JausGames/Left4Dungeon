using UnityEngine;
using L4P.Gameplay.Player.Animations;


namespace L4P.Gameplay.Weapons
{
    public class BulletRange : Weapon
    {
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;
        [SerializeField] GameObject bulletPrefab;

        public override void Use(bool performed, PlayerAnimatorController playerAnimator)
        {
            //do it with hash
            playerAnimator.SetAttackAnimation(performed);
            if (performed && NextHit <= Time.time)
            {
                NextHit = Time.time + stats.cooldown;
                foreach (var prtcl in shotPrtcls)
                    prtcl.Play();

                var bulletObj = Instantiate(bulletPrefab, startingPoint.position, Owner.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetUp(stats.damage, stats.range, ennemyLayer);
            }
        }
    }
}
