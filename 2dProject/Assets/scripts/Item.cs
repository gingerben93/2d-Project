using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {MANA, HEALTH}; //add more types here for items

public class Item : MonoBehaviour {

    public ItemType type;

    public Sprite spriteNeutral;

    public Sprite spriteHighlighted;

    public int maxSize;

    public void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:
                Debug.Log("Health potion was used");
                break;
            default:
                break;
        }
    }
}
