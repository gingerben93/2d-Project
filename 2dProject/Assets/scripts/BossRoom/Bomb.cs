using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public Rigidbody2D rb2d;
    public GameObject shield;
    public MoveTowardPlayer movement;

    void Start()
    {
        rb2d = GameObject.Find("Boss").GetComponent<Rigidbody2D>();

        //Access to gameobjects on boss
        shield = BossScript.BossScriptSingle.shield;
        movement = BossScript.BossScriptSingle.GetComponent<MoveTowardPlayer>();

        //No more bombs
        BossScript.BossScriptSingle.bombSpawn = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            rb2d.AddForce(new Vector2(0f, 1000f));
            Destroy(gameObject);

            //turn off shield and turn on movement
            shield.SetActive(false);
            movement.enabled = false;

            //One bomb at a time
            BossScript.BossScriptSingle.bombSpawn = false;


            //turnoff shield and make it so another bomb can spawn next shield phases.
            BossScript.BossScriptSingle.shieldOn = false;
            BossScript.BossScriptSingle.bombSpawn = false;

            //No longer stun the player
            PlayerController.PlayerControllerSingle.freezePlayer = false;
            if (GameObject.FindGameObjectWithTag("EnemyProjectile") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("EnemyProjectile"));
            }
        }

    }
}
