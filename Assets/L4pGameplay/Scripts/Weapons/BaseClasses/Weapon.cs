using System.Collections;
using UnityEngine;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Weapons
{
    abstract public class Weapon : MonoBehaviour
    {
        [SerializeField] protected int leftAnimTriggerHash;
        [SerializeField] protected int rightAnimTriggerHash;
        [SerializeField] protected WeaponStat stats = null;
        [SerializeField] protected Stance stance;



        public WeaponStat Stats { get => stats; }

        float nextHit = 0f;
        [SerializeField] protected Transform owner;
        public float NextHit { get => nextHit; set => nextHit = value; }
        public Transform Owner { get => owner; set => owner = value; }
        private void Awake()
        {
            Owner = GetComponentInParent<Animator>().transform;
        }

        public abstract void Use(bool performed, PlayerAnimatorController playerAnimator);
    }

    [System.Serializable]
    public class WeaponStat
    {
        public float damage = 10f;
        public float range = 10f;
        public float cooldown = .7f;
        public float knockback = 1f;
        public float knockTime = .5f;
    }

    public enum Stance
    {
        Left,
        Right,
        TwoHands
    }

}