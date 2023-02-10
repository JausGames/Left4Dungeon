using System.Collections;
using UnityEngine;
using L4P.Gameplay.Weapons;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        [SerializeField] public PlayerAnimatorController animator;
        [SerializeField] public PlayerAnimatorEvent animatorEvent;

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
            weapon.Use(performed, animator);
            //animator.SetAttackAnimationTrigger(performed);
        }
        private void Update()
        {
            if(useWeapon)
            {
                if (isRightHand && currentRightHand.NextHit <= Time.time)
                {
                    currentRightHand.Use(true, animator);
                }
                else if (!isRightHand && currentRightHand.NextHit <= Time.time)
                {
                    currentLeftHand.Use(true, animator);
                }
            }
        }
        private void Awake()
        {
            //currentWeapon = GetComponentInChildren<IWeapon>();
            animator = GetComponent<PlayerAnimatorController>();
            animatorEvent = GetComponent<PlayerAnimatorEvent>();

            //animatorEvent.LeftActivateEvent.AddListener(currentLeftHand)
        }
    }
}