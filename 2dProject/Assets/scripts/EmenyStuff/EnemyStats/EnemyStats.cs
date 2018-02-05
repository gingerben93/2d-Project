using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{

    //List of transforms to hold droppable items
    public List<Transform> items = new List<Transform>();

    //drop rate of an item
    private float dropRate = .50f;

    //drop chance for an item
    private float dropChance;

    //int to choose item from list
    private int choice;

    /// Total hitpoints
    public int health = 2;
    private int experiencePoint = 5;

    //invinsible
    public bool invincible = false;

    void Start()
    {
        // set emeny information
        experiencePoint = 5;
        
    }

    //getting name is for seeing which part of the game object was hit if it has children
    //and special interation is required
    public virtual void Damage(int damageCount, string NameObject)
    {
        //Debug.Log("NameObject = " + NameObject);
        if (!invincible)
        {
            health -= damageCount;

            if (health <= 0)
            {
                DeathPhase();
            }
        }
    }


    public void DeathPhase()
    {
        //set exp
        PlayerStats.PlayerStatsSingle.GainExperiencePoints(experiencePoint);
        //PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

        //check kill quest
        KillQuest[] listKillQuests = QuestController.QuestControllerSingle.GetComponentsInChildren<KillQuest>();
        if (listKillQuests.Length > 0)
        {
            foreach (KillQuest quest in listKillQuests)
            {
                if (gameObject.name == quest.killTarget)
                {
                    quest.killQuestCounter += 1;
                    quest.UpdateKillQuest();
                }
            }
        }

        if (items.Count > 0)
        {
            DropItems();
        }

        // Dead!
        Destroy(gameObject);
    }

    public void DropItems()
    {
        //could load in items by finding name like this; have to put all items that want to be loading in a similar place
        //Debug.Log(Application.dataPath);
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/prefab/Drops/Armor");
        //FileInfo[] fi = di.GetFiles();
        //foreach (FileInfo f in fi)
        //{
        //    if(f.Extension == ".prefab")
        //        Debug.Log("name = " + f.Name);
        //}

        Instantiate(items[0], transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;

        dropChance = Random.Range(0, 1.0f);

        if (dropChance < dropRate)
        {
            choice = Random.Range(0, items.Count);
            Debug.Log("items.count = " + items.Count + " choice = " + choice);
            Instantiate(items[choice], transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
        }
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player" && !PlayerController.PlayerControllerSingle.invincible)
        //Debug.Log("parent = " + gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("name object = " + gameObject.name);
            PlayerController.PlayerControllerSingle.DamagePlayer(1);
        }
    }

    //void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    //if bullet, do bullet stuff
    //    if (otherCollider.tag == "Bullet")
    //    {
    //        Destroy(otherCollider.gameObject);

    //        if (!invincible)
    //        {
    //            Damage(PlayerController.PlayerControllerSingle.weaponDamage);
    //        }
    //    }
    //}
}
