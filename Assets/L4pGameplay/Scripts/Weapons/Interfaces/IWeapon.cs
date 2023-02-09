using System.Collections;
using UnityEngine;

namespace L4P.Gameplay.Weapons.Interfaces
{
    public interface IWeapon
    {
        public WeaponStat Stats { get; }
        public float NextHit { get; }
        public void Use(bool performed);
    }

    [System.Serializable]
    public class WeaponStat
    {
        public float damage = 10f;
        public float range = 10f;
        public float cooldown = .7f;
    }
}