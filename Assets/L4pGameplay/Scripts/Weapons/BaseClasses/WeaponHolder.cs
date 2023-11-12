using L4P.Gameplay.Player;
using L4P.Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : Pickable
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject ui;
    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI label;

    public GameObject Prefab {
        get => prefab;
        set 
        {
            prefab = value;
            var weapon = value.GetComponent<Weapon>();
            image.sprite = weapon.sprite;
            label.text = weapon.name;
        }
    }

    public override void Interact(Player player)
    {
        // So it doesn't destroy gameobject if X pressed
    }
}
