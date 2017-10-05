using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    Rigidbody2D rbd2OfItem;
    bool attract = false;

    void Start()
    {
        rbd2OfItem = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (attract)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, PlayerController.PlayerControllerSingle.transform.position, .125f);
            if (Vector2.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position) <= 1f)
            {
                //check with all gather quest that item was needed
                GatherQuest[] listGatherQuests = QuestController.QuestControllerSingle.GetComponentsInChildren<GatherQuest>();
                if (listGatherQuests.Length > 0)
                {
                    foreach( GatherQuest quest in listGatherQuests)
                    {
                        if(gameObject.name == quest.gatherTarget)
                        {
                            quest.gatherQuestCounter += 1;
                            quest.UpdateGatherQuest();
                        }
                    }
                }

                //put item in iventory
                Inventory.InventorySingle.AddItem(gameObject.GetComponent<Item>());

                //destory item
                Destroy(gameObject);
            }
        }
        else if (Vector3.Distance(gameObject.transform.position, PlayerController.PlayerControllerSingle.transform.position) < 10f)
        {
            rbd2OfItem.simulated = false;
            attract = true;
        }
    }
}
