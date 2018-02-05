using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item {

    public override void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:
                if (PlayerStats.PlayerStatsSingle.health <= PlayerStats.PlayerStatsSingle.maxHealth)
                {
                    PlayerStats.PlayerStatsSingle.health += 1;
                    Debug.Log("Health potion was used");
                }
                else
                {
                    Debug.Log("Full health, fuking idiot, die potion");
                }

                break;
            default:
                break;
        }
    }

}
