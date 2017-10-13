using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    public string playerName { get; set; }

    public bool facingRight { get; set; }
    public bool jump { get; set; }

    public float attack { get; set; }
    public float moveForce { get; set; }
    public float maxSpeed { get; set; }
    public float jumpForce { get; set; }

    //dash force
    float dashForce = 1000f;

    //rotate with dash
    private bool rotateRight = false;
    private bool rotateleft = false;
    float rotateSpeed = 20f;
    float lastRotationAngle = 0f;

    //double tap click event
    private float ButtonCooler = 0.5f; // Half a second before reset
    private int ButtonCount = 0;
    private float dashCoolDown = 0.0f;

    //touching anything
    public bool IsColliding { get; set; }

    //player rigidbody
    public Rigidbody2D rb2d;

    //for door colliding script
    DoorCollider doorInfo;

    //for respawning
    public Vector3 respawnLocation;

    //for freezeing player
    public bool freezePlayer { get; set; }

    //for falling
    private bool falling = false;
    private float timer = 0;

    //for weapons
    public int weaponDamage;

    //skillBoolCheck
    public bool dashSkill = false;
    public bool dashSkill2 = false;

    //for hotbar
    public delegate void HotBarDelegate();
    public HotBarDelegate HotBarSlot1, HotBarSlot2, HotBarSlot3;

    //for player attack
    public delegate void PlayerAttack();
    public PlayerAttack playerAttack;

    //for slide move
    private EdgeCollider2D edge;
    private int currentEdgePoint = 0;
    private Vector2 CurrentVector;
    private Vector2 knownVec;
    private Vector2 perpVec;
    private Quaternion targetRotation;

    //is touching door
    public bool touchingDoor { get; set; }

    //invinsible
    private bool invincible = false;
    //attack cooldown when destoryed
    float InvinicbleCoolDown = 2;
    float InvinicbleCoolDownTimer = 0;

    //for fade in and out player sprite
    SpriteRenderer PlayerSprite;

    //singleton
    public static PlayerController PlayerControllerSingle;

    void Awake()
    {
        if (PlayerControllerSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            PlayerControllerSingle = this;
        }
        else if (PlayerControllerSingle != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        IsColliding = false;
        freezePlayer = false;

        //variables on start
        maxSpeed = 7f;
        moveForce = 5f;
        jumpForce = 500;
        facingRight = true;
        jump = false;
        attack = 2;

        //other start stuff
        doorInfo = GetComponent<DoorCollider>();
        rb2d = GetComponent<Rigidbody2D>();

        //for where player is at start of scene
        transform.position = respawnLocation;

        //start player name
        playerName = "fukin nerd";

        //set sprite
        PlayerSprite =  transform.GetComponentInChildren<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {



        //for stoping all player actions
        if (freezePlayer)
        {
            return;
        }

        //if player health hits 0
        if (PlayerStats.PlayerStatsSingle.health <= 0)
        {
            transform.position = respawnLocation;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }

        //going through doors
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (touchingDoor)
            {
                GameController.GameControllerSingle.RemoveCurrentMapObjects();

                MapGenerator.MapGeneratorSingle.seed = GameController.GameControllerSingle.mapSeed;
                MapGenerator.MapGeneratorSingle.LoadMap();

                //for minimap
                DrawPlayerMap.DrawPlayerMapSingle.UpdateMap();

                Vector2 door;
                door = MapGenerator.MapGeneratorSingle.doorLocations[doorInfo.numVal];
                transform.position = new Vector3(door.x, door.y, 0);

                respawnLocation = door;
            }
        }

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && rb2d.velocity.y < maxSpeed)
        {
            jump = true;
            rb2d.gravityScale = 1;
        }

        //button 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (HotBarSlot1 != null)
            {
                HotBarSlot1();
            }
        }

        //button 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (HotBarSlot2 != null)
            {
                HotBarSlot2();
            }
        }

        //button 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (HotBarSlot3 != null)
            {
                HotBarSlot3();
            }
        }

        //for rotating during roll
        if (rotateRight)
        {
            //euler angle start at 360 when going right
            transform.Rotate(Vector3.back, rotateSpeed);
            if (lastRotationAngle < transform.eulerAngles.z)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                rotateRight = false;
            }
            lastRotationAngle = transform.eulerAngles.z;
        }

        if (rotateleft)
        {
            //euler angle start at 0 when going left
            transform.Rotate(Vector3.forward, rotateSpeed);
            if (lastRotationAngle > transform.eulerAngles.z)
            {
                transform.eulerAngles = Vector3.zero;
                rotateleft = false;
            }
            lastRotationAngle = transform.eulerAngles.z;
        }

        //have to learn dash skill to start using it
        if (dashSkill && dashSkill2)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Number of Taps you want Minus One
                if (ButtonCooler > 0.0f && ButtonCount == 1 && dashCoolDown <= 0f)
                {
                    Debug.Log("double tap right");
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x >= 6f)
                    {
                        Debug.Log("worked");
                        rb2d.velocity = new Vector2(-7f, rb2d.velocity.y);
                    }
                    else {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
                    }
                    rb2d.AddForce(new Vector2(dashForce, 0f));

                    //for rotating teemo
                    rotateleft = false;
                    rotateRight = true;
                    lastRotationAngle = 360f;
                }
                else
                {
                    ButtonCooler = 0.2f;
                    ButtonCount += 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //Number of Taps you want Minus One
                if (ButtonCooler > 0.0f && ButtonCount == 1 && dashCoolDown <= 0f)
                {
                    Debug.Log("double tap left");
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x <= -6f)
                    {
                        Debug.Log("worked");
                        rb2d.velocity = new Vector2(7f, rb2d.velocity.y);
                    }
                    else {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
                    }
                    rb2d.AddForce(new Vector2(-dashForce, 0f));

                    //for rotating teemo
                    rotateRight = false;
                    rotateleft = true;
                    lastRotationAngle = 0f;
                }
                else
                {
                    ButtonCooler = 0.2f;
                    ButtonCount += 1;
                }
            }

            if (ButtonCooler > 0.0f)
            {
                ButtonCooler -= 1.0f * Time.deltaTime;
            }
            else
            {
                ButtonCount = 0;
            }

            if (dashCoolDown > 0.0f)
            {
                dashCoolDown -= Time.deltaTime;
            }
        }

        //stop all action after this if the pointer is over the canvas
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem.IsPointerOverGameObject())
        {
            return;
        }

        else if (Input.GetMouseButtonDown(1))
        {
            if (playerAttack != null)
            {
                playerAttack();
            }
        }
    }

    void FixedUpdate()
    {
        if (invincible)
        {
            //cool down for attack
            if (InvinicbleCoolDownTimer > 0)
            {
                InvinicbleCoolDownTimer -= Time.deltaTime;
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.b, PlayerSprite.color.g, (PlayerSprite.color.a + .04f) % 1f);
            }
            else
            {
                InvinicbleCoolDownTimer = 0;
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.b, PlayerSprite.color.g, 1f);
                invincible = false;
            }
        }


        // stops all player actions
        if (freezePlayer)
        {
            return;
        }

        //if edge collider is there 
        if (edge)
        {
            if (Input.GetKey(KeyCode.S))
            {
                rb2d.gravityScale = 0;
                CurrentVector = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);

                //gets current perp vector with current and next point
                knownVec = CurrentVector - new Vector2(edge.points[(currentEdgePoint + 1) % edge.pointCount].x, edge.points[(currentEdgePoint + 1) % edge.pointCount].y);
                perpVec = CurrentVector + new Vector2(knownVec.y, -knownVec.x) * 1;

                //for moving the object
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, perpVec, .25f);

                //for rotating the object
                targetRotation = Quaternion.LookRotation(Vector3.forward, perpVec - CurrentVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .25f);

                //gets next point to move to
                if (Vector3.Distance(gameObject.transform.position, perpVec) <= .1f)
                {
                    if (facingRight)
                    {
                        currentEdgePoint = (currentEdgePoint + 1) % (edge.edgeCount - 1);
                    }
                    else
                    {
                        currentEdgePoint = ((edge.edgeCount - 1) + currentEdgePoint - 1) % (edge.edgeCount - 1);
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                rb2d.gravityScale = 1;
                transform.eulerAngles = Vector3.zero;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            //flip character sprite
            if (!facingRight)
            {
                Flip();
            }
            //if going other direction, stop
            if (rb2d.velocity.x < 0)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            //speed up by 1 to the right
            if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x + .5f, rb2d.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //flip character sprite
            if (facingRight)
            {
                Flip();
            }
            //if going other direction, stop
            if (rb2d.velocity.x > 0)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            //speed up by 1 to the left
            if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x - .5f, rb2d.velocity.y);
            }
        }
        //if colliding then slow down (don't slow doing in air)
        else if (IsColliding == true)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * .5f, rb2d.velocity.y);
        }
        //else slow down by a little
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * .97f, rb2d.velocity.y);
        }

        if (Mathf.Abs(rb2d.velocity.y) >= maxSpeed)
        {
            //start timer for falling; take damage after 1 sec
            //deltatime is time sinece last frame.
            timer += Time.deltaTime;
            falling = true;
        }
        else
        {
            falling = false;
            timer = 0;
        }

        if (jump)
        {
            rb2d.gravityScale = 1;
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
            }
            if (Mathf.Sign(rb2d.velocity.y) < 1.2f)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
            }
            if (Mathf.Sign(rb2d.velocity.y) > 1.2f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * 0.2f);
            }
            jump = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void LockPosition()
    {
        freezePlayer = true;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    public void UnLockPosition()
    {
        freezePlayer = false;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.transform.rotation = Quaternion.identity;
    }

    private void SetInvisible()
    {
        invincible = true;
        InvinicbleCoolDownTimer = InvinicbleCoolDown;
    }

    public void DamagePlayer(int DamageAmount)
    {
        if (!invincible)
        {
            SetInvisible();
            PlayerStats.PlayerStatsSingle.health -= DamageAmount;
        }
    }

    void OnTriggerEnter2D(Collider2D triggerCollition)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsColliding = true;
        
        //moved this into magnet script for testing stuff
        //add item to inventory
        //if (collision.gameObject.tag == "Item")
        //{
        //    Inventory.InventorySingle.AddItem(collision.gameObject.GetComponent<Item>());
        //    Destroy(collision.gameObject);
        //}

        //falling damage
        if (falling && timer >= 2f)
        {
            DamagePlayer(1);
        }

        //for slide move; gets edge collider player is touching
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            if (collision.gameObject.GetComponent<EdgeCollider2D>())
            {
                edge = collision.gameObject.GetComponent<EdgeCollider2D>();
                for (int index = 0; index < edge.pointCount; index++)
                {
                    if (Vector2.Distance(edge.points[index], collision.contacts[0].point) <= 1f)
                    {

                        currentEdgePoint = index;
                        return;
                    }
                }
            }
        }

        //for taking damage
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit enemy = " + collision.gameObject.name);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsColliding = false;

        //for slide move
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            edge = null;
        }
    }
}
