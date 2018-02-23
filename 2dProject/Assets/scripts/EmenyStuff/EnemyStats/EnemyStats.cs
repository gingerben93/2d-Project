using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{
    //List of transforms to hold droppable items
    public List<Transform> items = new List<Transform>();

    //Set these for different enemies;
    public float dropRate;
    public bool damagePlayerOnCollision;
    public int health;
    public int experiencePoint;
    public bool invincible;

    //drop chance for an item
    private float dropChance;

    //int to choose item from list
    private int choice;

    void Start()
    {
        StartStats();
    }

    public virtual void StartStats()
    {
        dropRate = 0;
        damagePlayerOnCollision = true;
        health = 1;
        experiencePoint = 1;
        invincible = false;
    }

    //getting name is for seeing which part of the game object was hit if it has children
    //and special interation is required
    public virtual void Damage(int damageCount, string NameObject, float InvicTime)
    {
        Debug.Log("NameObject = " + NameObject);
        if (!invincible)
        {
            health -= damageCount;

            if (health <= 0)
            {
                DeathPhase();
            }
        }
    }

    public IEnumerator MakeInvincible(float it)
    {
        invincible = true;
        yield return new WaitForSeconds(it);
        invincible = false;
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

        //creates small variance on spawn location
        Vector3 SpawnLocation = new Vector3(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 0);
        //spawn first thing in itesms which is health essence
        Instantiate(items[0], transform.position + SpawnLocation, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;

        dropChance = Random.Range(0.0f, 1.0f);

        if (dropChance >= dropRate)
        {
            SpawnLocation = new Vector3(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 0);
            choice = Random.Range(0, items.Count);
            Instantiate(items[choice], transform.position + SpawnLocation, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (damagePlayerOnCollision)
        {
            //Debug.Log("parent = " + gameObject.name);
            if (collision.gameObject.tag == "Player")
            {
                //Debug.Log("name object = " + gameObject.name);
                PlayerController.PlayerControllerSingle.DamagePlayer(1);
            }
        }
    }

    //can use dame here to hit certain parts of objects
    //void OnTriggerEnter2D(Collider2D Trigger)
    //{
    //    Debug.Log(gameObject.name);
    //    Damage(100, Trigger.name);
    //}
}
