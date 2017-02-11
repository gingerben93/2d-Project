﻿using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{
    public Transform loot;

    Shot shot;

    /// Total hitpoints
    public int hp { get; set; }
    public bool isEnemy { get; set; }
    private int experiencePoint;

    //for giving experiencePoint
    public GameController GameCon;

    void Start()
    {
        // make reference to gameCon
        GameCon = FindObjectOfType<GameController>();

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
            GameCon.experiencePoint += experiencePoint;

            Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform; ;
            // Dead!
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        shot = otherCollider.gameObject.GetComponent<Shot>();
        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(shot.damage);

                // Destroy the shot
                Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
    }
}
