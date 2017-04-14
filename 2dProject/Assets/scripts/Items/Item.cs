using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH, WEAPON, SHIELD, HELM, CURIASS, BELT, GLOVES, BOOTS, RING, AMULET,  KEY}; //add more types here for items
public enum Quality { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT }

public class Item : MonoBehaviour {

    public ItemType type;
    public Quality quality;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    Inventory inventory;

    public int maxSize;

    private static Slot weap;
    private GameObject slot;


    //higher and lower bounds for armor
    public int armorL;
    public int armorH;
    public int armor;

    //higher and lower bounds for attack
    public int damageL;
    public int damageH;
    public int damage;

    //higher and lower bounds for vitality
    public int vitL;
    public int vitH;
    public int vit;

    //higher and lower bounds for intelligence
    public int intelligenceL;
    public int intelligenceH;
    public int intelligence;

    //higher and lower bounds for strength
    public int strengthL;
    public int strengthH;
    public int strength;

    //higher and lower bounds for charisma
    public int charismaL;
    public int charismaH;
    public int charisma;

    //item information
    public string weaponName;
    public string description;


    void Start()
    {
        armor = Random.Range(armorL, armorH);
        damage = Random.Range(damageL, damageH);
        vit = Random.Range(vitL, vitH);
        intelligence = Random.Range(intelligenceL, intelligenceH);
        strength = Random.Range(strengthL, strengthH);
        charisma = Random.Range(charismaL, charismaH);
    }

    public void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:
                if(PlayerStats.PlayerStatsSingle.health <= PlayerStats.PlayerStatsSingle.maxHealth)
                {
                    PlayerStats.PlayerStatsSingle.health += 1;
                    Debug.Log("Health potion was used");
                }
                else
                {
                    Debug.Log("Full health, fuking idiot, die potion");
                }

                break;
            case ItemType.WEAPON:
                //atk = EquipStats.EquipStatsSingle.RandomStat(EquipStats.EquipStatsSingle.atkL, EquipStats.EquipStatsSingle.atkH);
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

    public string GetToolTip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if(description != string.Empty)
        {
            newLine = "\n";
        }

        switch (quality)
        {
            case Quality.COMMON:
                color = "white";
                break;
            case Quality.UNCOMMON:
                color = "lime";
                break;
            case Quality.RARE:
                color = "navy";
                break;
            case Quality.EPIC:
                color = "magenta";
                break;
            case Quality.LEGENDARY:
                color = "orange";
                break;
            case Quality.ARTIFACT:
                color = "red";
                break;
            default:
                break;
        }

        if(damage > 0)
        {
            stats += "\n" + damage.ToString() + " Damage";
        }
        if (armor > 0)
        {
            stats += "\n" + armor.ToString() + " Armor";
        }
        if(vit > 0)
        {
            stats += "\n" + vit.ToString() + " Vitality";
        }
        if (intelligence > 0)
        {
            stats += "\n" + intelligence.ToString() + " Intelligence";
        }
        if (charisma > 0)
        {
            stats += "\n" + charisma.ToString() + " Charisma";
        }

        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color><i/>{2}<s/ize>", weaponName, description, stats);
    }


}
