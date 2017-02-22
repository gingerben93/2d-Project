﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH, MWEAPON, RWEAPON}; //add more types here for items

public class Item : MonoBehaviour {

    public ItemType type;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    Inventory inventory;

    public int maxSize;

    private static int attack;

    public void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:
                Debug.Log("Health potion was used");
                break;
            case ItemType.MWEAPON:
                break;
            case ItemType.RWEAPON:
                break;
            default:
                break;
        }
    }
}
