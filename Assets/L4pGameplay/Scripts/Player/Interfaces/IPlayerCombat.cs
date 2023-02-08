using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Weapons.Interfaces;

namespace L4P.Gameplay.Player
{
    public interface IPlayerCombat
    {
        public IWeapon CurrentWeapon { get; }
        public void UseWeapon(bool performed);
    }
}
