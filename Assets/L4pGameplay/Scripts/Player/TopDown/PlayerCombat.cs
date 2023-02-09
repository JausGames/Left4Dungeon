using System.Collections;
using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;
using L4P.Gameplay.Weapons;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        [SerializeField] public PlayerAnimatorController animator;

        private IWeapon currentWeapon;
        public IWeapon CurrentWeapon { get => currentWeapon; }

        public bool useWeapon = false;

        public void UseWeapon(bool performed)
        {
            useWeapon = performed;
            currentWeapon.Use(performed);
            animator.SetAttackAnimation(performed);
        }
        private void Update()
        {
            if(useWeapon && currentWeapon.NextHit <= Time.time)
            {
                currentWeapon.Use(true);
            }
        }
        private void Awake()
        {
            currentWeapon = GetComponentInChildren<IWeapon>();
            animator = GetComponent<PlayerAnimatorController>();
        }
    }
}