﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    //private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    //is touching door
    public bool touchingDoor = false;

    public string mapSeed;
    public int doorRef;


    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        MapGenerator map = FindObjectOfType<MapGenerator>();
        Vector2 door;
        door = map.doorLocations[doorRef];
        transform.position = new Vector3(door.x, door.y, 0);
    }

	
	// Update is called once per frame
	void Update () {
        //grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        Resources.UnloadUnusedAssets();
        if (Input.GetKeyDown(KeyCode.R) && touchingDoor)
        {
            MapGenerator map = FindObjectOfType<MapGenerator>();
            DoorCollider doorInfo = FindObjectOfType<DoorCollider>();
            DrawPlayerMap changeLocalMap = FindObjectOfType<DrawPlayerMap>();

            map.seed = mapSeed;
            map.GenerateMap();
            changeLocalMap.touchingDoor = true;
            //Start();
            Vector2 door;
            door = map.doorLocations[doorInfo.numVal];
            transform.position = new Vector3(door.x, door.y, 0);


        }

        if (Input.GetButtonDown("Jump") /*&& grounded*/)
        {
            jump = true;
        }

         // 5 - Shooting
         bool shoot = Input.GetButtonDown("Fire1");
         shoot |= Input.GetButtonDown("Fire2");
         // Careful: For Mac users, ctrl + arrow is a bad idea
         
        if (shoot)
        {
            Weapon weapon = GetComponent<Weapon>();
            if (weapon != null)
            {
                // false because the player is not an enemy
                weapon.Attack(false);
            }
        }
    }




    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.velocity = new Vector2(h * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();

        if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            if (Mathf.Sign(rb2d.velocity.y) < 1.2f)
                rb2d.AddForce(new Vector2(0f, jumpForce));
            if (Mathf.Sign(rb2d.velocity.y) > 1.2f)
                rb2d.velocity = new Vector2(0, Mathf.Sign(rb2d.velocity.y) * 0.2f);
            jump = false;
        }

        if (Mathf.Sign(rb2d.velocity.x) > 15f)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * 0.995f, rb2d.velocity.y);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
