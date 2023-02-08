using System.Collections;
using UnityEngine;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(Animation.PlayerAnimatorController))]
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        public void UseWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}