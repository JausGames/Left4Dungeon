using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Weapons;
using L4P.Gameplay.Player.TopDown;

namespace L4P.Gameplay.Player
{
    public interface IPlayerCombat
    {
        public Weapon CurrentRightHand { get; }
        public Weapon CurrentLeftHand { get; }
        public void UseWeapon(bool performed, AttackType type);
    }
}
