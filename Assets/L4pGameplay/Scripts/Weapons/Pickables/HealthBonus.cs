using L4P.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : Pickable
{
    [SerializeField] float amount;
    public override void Interact(Player player)
    {
        player.RecoverHealth(amount);
        base.Interact(player);
    }
}
