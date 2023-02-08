using System.Collections;
using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;
using L4P.Gameplay.Weapons;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(Animation.PlayerAnimatorController))]
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {

        private IWeapon currentWeapon;
        public IWeapon CurrentWeapon { get => currentWeapon; }

        public void UseWeapon(bool performed)
        {
           currentWeapon.Use(performed);
        }
        private void Awake()
        {
            currentWeapon = GetComponentInChildren<IWeapon>();
        }
    }
}