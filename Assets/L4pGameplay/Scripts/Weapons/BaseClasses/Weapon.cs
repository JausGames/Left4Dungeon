using System.Collections;
using UnityEngine;
using L4P.Gameplay.Player.Animations;
using UnityEngine.UI;

namespace L4P.Gameplay.Weapons
{
    abstract public class Weapon : MonoBehaviour
    {
        [SerializeField] protected int leftAnimTriggerHash;
        [SerializeField] protected int rightAnimTriggerHash;
        [SerializeField] protected WeaponStat stats = null;
        [SerializeField] protected new string name = null;



        public WeaponStat Stats { get => stats; }

        float nextHit = 0f;
        [SerializeField] protected Transform owner;
        [SerializeField] internal Sprite sprite;

        public float NextHit { get => nextHit; set => nextHit = value; }
        public Transform Owner { get => owner; set => owner = value; }

        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        internal GameObject Prefab { get => Resources.Load<GameObject>("Weapons/" + name); }

        private void Awake()
        {
            Owner = GetComponentInParent<Animator>().transform;
        }

        public abstract void Use(bool performed, PlayerAnimatorController playerAnimator);
        public abstract void UseWeak(bool performed, PlayerAnimatorController playerAnimator);
        public abstract void UseStrong(bool performed, PlayerAnimatorController playerAnimator);
    }

    [System.Serializable]
    public class WeaponStat
    {
        public float damage = 10f;
        public float range = 10f;
        public float cooldown = .7f;
        public float knockback = 1f;
        public float knockTime = .5f;

        public WeaponStat(WeaponStat stats)
        {
            this.damage = stats.damage;
            this.range = stats.range;
            this.cooldown = stats.cooldown;
            this.knockback = stats.knockback;
            this.knockTime = stats.knockTime;
        }
    }

    public enum Stance
    {
        Left,
        Right,
        TwoHands
    }

}