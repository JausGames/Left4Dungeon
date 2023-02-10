using System.Collections;
using UnityEngine;

namespace L4P.Gameplay.Weapons.Interfaces
{
    abstract public class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponStat stats = null;
        public WeaponStat Stats { get => stats; }

        float nextHit = 0f;
        [SerializeField] protected Transform owner;
        public float NextHit { get => nextHit; set => nextHit = value; }
        public abstract void Use(bool performed);
    }

    [System.Serializable]
    public class WeaponStat
    {
        public float damage = 10f;
        public float range = 10f;
        public float cooldown = .7f;
    }
}