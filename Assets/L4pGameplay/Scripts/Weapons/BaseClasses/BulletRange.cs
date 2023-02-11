using UnityEngine;
using L4P.Gameplay.Player.Animations;


namespace L4P.Gameplay.Weapons
{
    public class BulletRange : RangeWeapon
    {
        [SerializeField] GameObject bulletPrefab;
        protected override void PerformShoot()
        {
            foreach (var prtcl in shotPrtcls)
                prtcl.Play();

            var bulletObj = Instantiate(bulletPrefab, startingPoint.position, Owner.rotation);
            var bullet = bulletObj.GetComponent<Bullet>();
            bullet.SetUp(stats.damage, stats.range, ennemyLayer);
        }
    }
}
