using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiptItem : Item {

    //higher and lower bounds for armor
    public int armorL;
    public int armorH;
    public int armor;

    //higher and lower bounds for dexterity
    public int dexterityL;
    public int dexterityH;
    public int dexterity;

    //higher and lower bounds for intelligence
    public int intelligenceL;
    public int intelligenceH;
    public int intelligence;

    //higher and lower bounds for strength
    public int strengthL;
    public int strengthH;
    public int strength;

    //higher and lower bounds for vitality
    public int vitalityL;
    public int vitalityH;
    public int vitality;

    //for melee skills
    public bool meleeWeapon;

    void Start()
    {
        //great little line for getting a random enum
        switch ((Quality)Random.Range(0, System.Enum.GetValues(typeof(Quality)).Length))
        {
            case Quality.COMMON:
                quality = Quality.COMMON;
                break;
            case Quality.UNCOMMON:
                quality = Quality.UNCOMMON;
                break;
            case Quality.RARE:
                quality = Quality.RARE;
                break;
            case Quality.EPIC:
                quality = Quality.EPIC;
                break;
            case Quality.LEGENDARY:
                quality = Quality.LEGENDARY;
                break;
            case Quality.ARTIFACT:
                quality = Quality.ARTIFACT;
                break;
            default:
                break;
        }

        itemName = gameObject.name;
        armor = Random.Range(armorL, armorH) + 2 * (int)quality;
        dexterity = Random.Range(dexterityL, dexterityH) + 2 * (int)quality;
        intelligence = Random.Range(intelligenceL, intelligenceH) + 2 * (int)quality;
        strength = Random.Range(strengthL, strengthH) + 2 * (int)quality;
        vitality = Random.Range(vitalityL, vitalityH) + 2 * (int)quality;
    }

    public override void Use()
    {
        PlayerStats.PlayerStatsSingle.UpdateStatText(armor, dexterity, intelligence, strength, vitality);
        switch (type)
        {
            case ItemType.WEAPON:
                Inventory.InventorySingle.SelectWeapon(itemName);
                if (meleeWeapon)
                {
                    PlayerController.PlayerControllerSingle.meleeEquipt = meleeWeapon;
                }
                break;
            case ItemType.HELM:
                break;
            case ItemType.CHEST:
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

    public override void UnEquipt()
    {
        //check for unequipt weapon
        if (type == ItemType.WEAPON)
        {
            Inventory.InventorySingle.UnequiptWeapon(itemName);

            //for player melee skills
            if (meleeWeapon)
            {
                PlayerController.PlayerControllerSingle.meleeEquipt = false;
            }
        }

        PlayerStats.PlayerStatsSingle.UpdateStatText(-armor, -dexterity, -intelligence, -strength, -vitality);
    }

    public override string GetToolTip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if (description != string.Empty)
        {
            newLine = "\n";
        }

        switch (quality)
        {
            case Quality.COMMON:
                color = "red";
                break;
            case Quality.UNCOMMON:
                color = "orange";
                break;
            case Quality.RARE:
                color = "yellow";
                break;
            case Quality.EPIC:
                color = "green";
                break;
            case Quality.LEGENDARY:
                color = "blue";
                break;
            case Quality.ARTIFACT:
                color = "purple";
                break;
            default:
                break;
        }

        if (armor > 0)
        {
            stats += "\n" + armor.ToString() + " Armor";
        }
        if (dexterity > 0)
        {
            stats += "\n" + dexterity.ToString() + " Dexterity";
        }
        if (intelligence > 0)
        {
            stats += "\n" + intelligence.ToString() + " Intelligence";
        }
        if (strength > 0)
        {
            stats += "\n" + strength.ToString() + " Damage";
        }
        if (vitality > 0)
        {
            stats += "\n" + vitality.ToString() + " Vitality";
        }

        return string.Format("<color=" + color + "><size=16>{0} {1}</size></color><size=14><i><color=lime>" + newLine + "{2}</color></i>{3}</size>", quality, itemName, description, stats);
    }
}
