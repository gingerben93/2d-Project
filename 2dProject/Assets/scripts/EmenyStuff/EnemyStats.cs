﻿using UnityEngine;
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
    public int hp { get; set; }
    public bool isEnemy { get; set; }
    private int experiencePoint;

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

        hp = 1;
        isEnemy = true;
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (hp <= 0)
        {
            //set exp
            PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

            //Increase Kill Counter
            if (QuestController.QuestControllerSingle.killQuestList.ContainsKey("Enemy"))
            {
                QuestController.QuestControllerSingle.KillQuestCounter += 1;
                QuestController.QuestControllerSingle.UpdateKillQuest("Enemy");
            }

            Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;

            dropChance = (float)Random.Range(0, 1.0f);

            if(dropChance < dropRate)
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        shot = otherCollider.gameObject.GetComponent<Shot>();
        shotM = otherCollider.gameObject.GetComponent<ShotM>();
        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(GameController.GameControllerSingle.damage);

                // Destroy the shot
                Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
        else if (shotM != null)
        {
            // Avoid friendly fire
            if (shotM.isEnemyShot != isEnemy)
            {
                Damage(GameController.GameControllerSingle.damage);

                // Destroy the shot
                Destroy(shotM.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
    }
}
