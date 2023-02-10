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

        [SerializeField] private Weapon currentRightHand;
        public Weapon CurrentRightHand { get => currentRightHand; }

        [SerializeField] private Weapon currentLeftHand;
        public Weapon CurrentLeftHand { get => currentLeftHand; }

        private bool useWeapon = false;
        private bool isRightHand = true;

        public void UseWeapon(bool performed, bool isRightHand)
        {
            this.isRightHand = isRightHand;
            useWeapon = performed;
            Weapon weapon = isRightHand ? currentRightHand : currentLeftHand;
            weapon.Use(performed);
            animator.SetAttackAnimation(performed);
        }
        private void Update()
        {
            if(useWeapon)
            {
                if (isRightHand && currentRightHand.NextHit <= Time.time)
                {
                    currentRightHand.Use(true);
                }
                else if (!isRightHand && currentRightHand.NextHit <= Time.time)
                {
                    currentLeftHand.Use(true);
                }
            }
        }
        private void Awake()
        {
            //currentWeapon = GetComponentInChildren<IWeapon>();
            animator = GetComponent<PlayerAnimatorController>();
        }
    }
}