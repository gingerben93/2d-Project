using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH, MWEAPON, RWEAPON, KEY}; //add more types here for items

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
                if(PlayerStats.playerStatistics.health <= PlayerStats.playerStatistics.maxHealth)
                {
                    PlayerStats.playerStatistics.health += 1;
                    Debug.Log("Health potion was used");
                }
                else
                {
                    Debug.Log("Full health, fuking idiot, die potion");
                }

                break;
            case ItemType.MWEAPON:
                break;
            case ItemType.RWEAPON:
                break;
            case ItemType.KEY:
                break;
            default:
                break;
        }
    }
}
