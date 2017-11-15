using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{
    public Transform loot; // 100% drop rate
    public Transform loot2; // 5% drop rate
    public Transform loot3; // 5% drop rate
    public Transform loot4; // 5% drop rate
    public Transform loot5; // 5% drop rate
    public Transform loot6; // 5% drop rate
    public Transform loot7; // 5% drop rate
    public Transform loot8; // 5% drop rate
    public Transform loot9; // 5% drop rate
    public Transform loot10; // 5% drop rate
    public Transform loot11; // 5% drop rate

    //List of transforms to hold droppable items
    public List<Transform> items = new List<Transform>();

    //drop rate of an item
    private float dropRate = .50f;

    //drop chance for an item
    private float dropChance;

    //int to choose item from list
    private int choice;

    Shot shot;
    ShotM shotM;

    /// Total hitpoints
    public int health;
    private int experiencePoint;

    //invinsible
    public bool invincible = false;

    void Start()
    {
        //10 items
        items.Add(loot2);
        items.Add(loot3);
        items.Add(loot4);
        items.Add(loot5);
        items.Add(loot6);
        items.Add(loot7);
        items.Add(loot8);
        items.Add(loot9);
        items.Add(loot10);
        items.Add(loot11);



        // set emeny information
        experiencePoint = 5;

        health = 2;
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        if (!invincible)
        {
            health -= damageCount;

            if (health <= 0)
            {
                //set exp
                PlayerStats.PlayerStatsSingle.GainExperiencePoints(experiencePoint);
                //PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

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

                Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;

                dropChance = Random.Range(0, 1.0f);

                if (dropChance < dropRate)
                {
                    choice = Random.Range(0, 10);
                    Instantiate(items[choice], transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
                }

                //Instantiate(loot2, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
                //Instantiate(loot3, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
                // Dead!
                Destroy(gameObject);
            }
        }
    }

    //public void Damage()
    //{
    //    Debug.Log("damage");
    //    if (!invincible)
    //    {
    //        Damage(PlayerController.PlayerControllerSingle.weaponDamage);
    //    }
    //}

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !PlayerController.PlayerControllerSingle.invincible)
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
