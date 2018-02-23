using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    Rigidbody2D rbd2OfItem;
    bool attract = false;
    bool deathPhase = false;
    public float attractDistance = 10f;
    public bool faceTarget;
    public float attractSpeed = .2f;

    public int magicChange;
    public int healthChange;

    void Start()
    {
        rbd2OfItem = gameObject.GetComponent<Rigidbody2D>();
        attractSpeed = Random.Range(.1f, .3f);
    }
    
    void LateUpdate()
    {
        if (attract && !deathPhase)
        {
            //face target
            if (faceTarget)
            {
                var dir = PlayerController.PlayerControllerSingle.transform.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            //move toward target
            transform.position = Vector3.MoveTowards(gameObject.transform.position, PlayerController.PlayerControllerSingle.transform.position, attractSpeed);

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
                if (gameObject.tag == "Item")
                {
                    Inventory.InventorySingle.AddItem(gameObject.GetComponent<Item>());
                }

                deathPhase = true;

                //if has particle system then do resource change
                if (gameObject.GetComponent<ParticleSystem>())
                {
                    gameObject.GetComponent<ParticleSystem>().Stop();
                    PlayerStats.PlayerStatsSingle.ChangeHealth(healthChange);
                    PlayerStats.PlayerStatsSingle.ChangeMagic(magicChange);
                    Destroy(gameObject, 1f);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (Vector3.Distance(gameObject.transform.position, PlayerController.PlayerControllerSingle.transform.position) < PlayerStats.PlayerStatsSingle.itemAttractDistance)
        {
            rbd2OfItem.simulated = false;
            attract = true;
        }
    }
}
