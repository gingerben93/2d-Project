using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH, WEAPON, SHIELD, HELM, CHEST, BELT, GLOVES, BOOTS, RING, AMULET, KEY}; //add more types here for items
public enum Quality { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT }

public class Item : MonoBehaviour {

    public ItemType type;
    public Quality quality;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    public int maxSize;

    //item information
    public string itemName;
    public string description;


    void Start()
    {
        if (maxSize == 0)
        {
            maxSize = 1;
        }
        itemName = gameObject.name;
        //armor = Random.Range(armorL, armorH);
        //damage = Random.Range(damageL, damageH);
        //vit = Random.Range(vitL, vitH);
        //intelligence = Random.Range(intelligenceL, intelligenceH);
        //strength = Random.Range(strengthL, strengthH);
        //charisma = Random.Range(charismaL, charismaH);
    }

    public virtual void Use()
    {
        Debug.Log("defualt use does nothing find override");
    }

    public virtual void UnEquipt()
    {
        Debug.Log("defualt unEquipt does nothing find override");
    }

    public virtual string GetToolTip()
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
        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i>{2}</size>", itemName, description, stats);
    }


}
