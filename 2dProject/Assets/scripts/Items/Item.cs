using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH, WEAPON, SHIELD, HELM, CURIASS, BELT, GLOVES, BOOTS, RING, AMULET,  KEY}; //add more types here for items

public class Item : MonoBehaviour {

    public ItemType type;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    public string weaponName;

    Inventory inventory;

    public int maxSize;

    private static int attack;





    private static Slot weap;
    private GameObject slot;
    public Item atk;

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
            case ItemType.WEAPON:
                break;
            case ItemType.KEY:
                break;
            case ItemType.HELM:
                break;
            case ItemType.CURIASS:
                break;
            case ItemType.BELT:
                break;
            case ItemType.GLOVES:
                break;
            case ItemType.BOOTS:
                break;
            case ItemType.RING:
                break;
            case ItemType.AMULET:
                break;
            default:
                break;
        }
    }

}
