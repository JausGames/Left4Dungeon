using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;


namespace L4P.Gameplay.Weapons
{
    public class BulletRange : MonoBehaviour, IWeapon
    {
        [SerializeField] LayerMask ennemyLayer;
        [SerializeField] WeaponStat stats = null;
        [SerializeField] ParticleSystem[] shotPrtcls;
        [SerializeField] Transform startingPoint;
        [SerializeField] GameObject bulletPrefab;
        public WeaponStat Stats { get; }

        public void Use(bool performed)
        {
            if (performed)
            {
                foreach (var prtcl in shotPrtcls)
                    prtcl.Play();

                //Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * stats.range, Color.red, 1f);

                var bulletObj = Instantiate(bulletPrefab, startingPoint.position, startingPoint.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetUp(stats.damage, stats.range, ennemyLayer);
            }
        }
    }
}
