using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;

namespace L4P.Gameplay.Weapons
{
    public class BasicRange : MonoBehaviour, IWeapon
    {
        WeaponStat stats = null;
        [SerializeField] ParticleSystem bulletPrtcl;
        public WeaponStat Stats { get; }

        public void Use(bool performed)
        {
            if(performed)
                bulletPrtcl.Play();
        }
    }
}
