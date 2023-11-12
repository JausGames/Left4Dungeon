using L4P.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable
{
    public override void Interact(Player player)
    {
        Destroy(gameObject);
    }
}
