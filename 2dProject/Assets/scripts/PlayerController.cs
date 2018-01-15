using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    public string playerName { get; set; }

    public bool facingRight { get; set; }

    //private float moveForce;
    private float maxSpeed;
    private float jumpForce;

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

    //hotbar cooldowns
    float hotBar1CoolDownTimer = 0;
    float hotBar2CoolDownTimer = 0;
    float hotBar3CoolDownTimer = 0;

    //for player attack
    public delegate void PlayerAttack();
    public PlayerAttack playerAttack;

    //for slide move
    private EdgeCollider2D currentEdge;
    private int currentEdgePoint = 0;
    private Vector2 CurrentVector;
    private Vector2 knownVec;
    private Vector2 perpVec;
    private Quaternion targetRotation;
    public bool IsSliding;

    private EdgeCollider2D[] Edges;
    bool firstRun = true;
    bool jumpOff = false;

    //for slide move particles
    private ParticleSystem SlideParticles;
    private ParticleSystem SlideJumpParticles;

    //is touching door
    public bool touchingDoor { get; set; }

    //invinsible
    public bool invincible { get; set; }
    //attack cooldown when destoryed
    float InvinicbleCoolDown = 2;
    float InvinicbleCoolDownTimer = 0;

    //for fade in and out player sprite
    SpriteRenderer PlayerSprite;

    //for smooth force movments
    public Vector2 forceAddToPlayer;

    //for grappling hook
    DrawGrapHook grapplingHookScript;

    //bools for inputs
    bool spaceDown;
    bool SHold;
    bool SUp;
    bool Mouse0Up;
    bool Mouse0Down;
    bool Mouse1Hold;
    bool DHold;
    bool AHold;

    // need to not add force with a and d when player is doing other stuff
    bool noMovement;

    //only one rotate call at a time
    bool isRotating;

    bool isGrapplingHook;


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
        //moveForce = 5f;
        jumpForce = 500;
        facingRight = true;

        //other start stuff
        doorInfo = GetComponent<DoorCollider>();
        rb2d = GetComponent<Rigidbody2D>();

        //for where player is at start of scene
        transform.position = respawnLocation;

        //start player name
        playerName = "fukin nerd";

        //set sprite
        PlayerSprite =  transform.GetComponentInChildren<SpriteRenderer>();

        //slide particle system
        SlideParticles = transform.Find("SlideSkill").transform.Find("ParticleSlide").GetComponentInChildren<ParticleSystem>();
        SlideJumpParticles = transform.Find("SlideSkill").transform.Find("ParticleJump").GetComponentInChildren<ParticleSystem>();

        //for gappling hook
        grapplingHookScript = GameObject.Find("GrapplingHook").GetComponent<DrawGrapHook>();
    }

    // Update is called once per frame
    void Update()
    {
        //for stoping all player actions
        if (freezePlayer)
        {
            return;
        }

        spaceDown = Input.GetKeyDown(KeyCode.Space);

        SUp = Input.GetKeyUp(KeyCode.S);
        SHold = Input.GetKey(KeyCode.S);

        Mouse0Up = Input.GetMouseButtonUp(0);
        Mouse0Down = Input.GetMouseButtonDown(0);

        Mouse1Hold = Input.GetMouseButton(1);

        DHold = Input.GetKey(KeyCode.D);
        AHold = Input.GetKey(KeyCode.A);


        //if player health hits 0
        if (PlayerStats.PlayerStatsSingle.health <= 0)
        {
            ResetPlayer();
            transform.position = respawnLocation;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }

        //going through doors
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (touchingDoor)
            {
                //reset player skills and stuff
                LockPosition();
                ResetPlayer();

                GameController.GameControllerSingle.RemoveEveryingOnMap();

                MapGenerator.MapGeneratorSingle.seed = GameController.GameControllerSingle.mapSeed;
                MapGenerator.MapGeneratorSingle.LoadMap();

                //for minimap
                DrawPlayerMap.DrawPlayerMapSingle.UpdateMap();

                Vector2 door;
                door = MapGenerator.MapGeneratorSingle.doorLocations[doorInfo.numVal];
                transform.position = new Vector3(door.x, door.y, 0);

                respawnLocation = door;
                UnLockPosition();
            }
        }

        if (Mouse0Down)
        {
            Debug.Log("Mouse 0 down");
            isGrapplingHook = true;
            grapplingHookScript.TurnGrapHookOn();
        }

        if (Mouse0Up)
        {
            if (isGrapplingHook)
            {
                isGrapplingHook = false;
                grapplingHookScript.TurnGrapHookOff();
                StartCoroutine(RotateToZero());
            }
        }

        //button 1
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (hotBar1CoolDownTimer > 0)
            {
                hotBar1CoolDownTimer -= Time.deltaTime;
            }
            else
            {
                if (HotBarSlot1 != null)
                {
                    HotBarSlot1();
                }
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
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x >= 6f)
                    {
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
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x <= -6f)
                    {
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

        else if (Mouse1Hold)
        {
            if (playerAttack != null)
            {
                playerAttack();
            }
        }
    }

    //don't put keybutton reads in here; they can hit twice
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
        if (SHold && !IsSliding)
        {
            try
            {
                if (firstRun)
                {
                    Edges = GameObject.Find("ColliderHolder").GetComponentsInChildren<EdgeCollider2D>();
                    float closestCollider = 5000;
                    foreach (EdgeCollider2D edge in Edges)
                    {
                        if (edge.Distance(transform.GetComponent<CapsuleCollider2D>()).distance < closestCollider)
                        {
                            closestCollider = edge.Distance(transform.GetComponent<CapsuleCollider2D>()).distance;

                            if (closestCollider <= 2)
                            {
                                currentEdge = edge;
                                firstRun = false;
                                noMovement = true;
                                IsSliding = true;
                                break;
                            }
                            else
                            {
                                closestCollider = 5000;
                                firstRun = true;
                            }
                        }
                    }
                    if (!firstRun && !jumpOff)
                    {
                        GetContactPoint();

                        rb2d.gravityScale = 0;
                        //turn on particles
                        SlideParticles.Play();
                    }
                }
            }
            catch
            {

            }
        }
        else if (SUp)
        {
            noMovement = false;
            IsSliding = false;
            firstRun = true;
            rb2d.gravityScale = 1;

            jumpOff = false;

            //turn off particles
            SlideParticles.Stop();
            SlideJumpParticles.Stop();

            StartCoroutine(RotateToZero());
        }

        if (IsSliding)
        {
            if (spaceDown && !jumpOff)
            {
                jumpOff = true;
                SlideJumpParticles.Play();
                rb2d.gravityScale = 0;
                rb2d.AddForce(transform.up * 25f);
            }
            else if (jumpOff)
            {
                if (DHold)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 2);
                }
                else if (AHold)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 2);
                }

                //if (rb2d.velocity.magnitude < 10f)
                //{
                //    rb2d.AddForce(transform.up * 25f);
                //}
                transform.position += transform.up * .25f;
            }
            //for rotating the object
            else if (!jumpOff)
            {
                //get new location to move to
                CurrentVector = new Vector2(currentEdge.points[currentEdgePoint].x, currentEdge.points[currentEdgePoint].y);

                //gets current perp vector with current and next point
                knownVec = CurrentVector - new Vector2(currentEdge.points[(currentEdgePoint + 1) % currentEdge.pointCount].x, currentEdge.points[(currentEdgePoint + 1) % currentEdge.pointCount].y);
                perpVec = CurrentVector + new Vector2(knownVec.y, -knownVec.x);
                //for moving the object
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, perpVec, .25f);

                targetRotation = Quaternion.LookRotation(Vector3.forward, perpVec - CurrentVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .25f);
            }


            //gets next point to move to
            if (Vector3.Distance(gameObject.transform.position, perpVec) <= .1f)
            {
                if (facingRight)
                {
                    currentEdgePoint = (currentEdgePoint + 1) % (currentEdge.edgeCount - 1);
                }
                else
                {
                    currentEdgePoint = ((currentEdge.edgeCount - 1) + currentEdgePoint - 1) % (currentEdge.edgeCount - 1);
                }
            }
        }

        if (DHold)
        {
            //flip character sprite
            if (!facingRight)
            {
                Flip();
            }
            if (!noMovement)
            {
                //if going other direction, stop
                if (rb2d.velocity.x < 0)
                {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
                //speed up by 1 to the right
                if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x + .5f, rb2d.velocity.y);

                    //rb2d.velocity += (Vector2)transform.right;

                    //forceAddToPlayer = transform.right * .5f;
                    //rb2d.velocity += forceAddToPlayer;
                }
            }
        }
        else if (AHold)
        {
            //flip character sprite
            if (facingRight)
            {
                Flip();
            }
            if (!noMovement)
            {
                //if going other direction, stop
                if (rb2d.velocity.x > 0)
                {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
                //speed up by 1 to the left
                if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x - .5f, rb2d.velocity.y);

                    //rb2d.velocity += -(Vector2)transform.right;

                    //forceAddToPlayer = -transform.right * .5f;
                    //rb2d.velocity += forceAddToPlayer;
                }
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

        if (spaceDown && !jumpOff)
        {
            rb2d.gravityScale = 1;
            if (isGrapplingHook)
            {
                if (facingRight)
                {
                    rb2d.AddForce(transform.right * jumpForce);
                }
                else
                {
                    rb2d.AddForce(-transform.right * jumpForce);
                }
            }
            else
            {
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
            }
        }

        if (spaceDown)
        {
            //turn off grappling hook
            if (isGrapplingHook)
            {
                isGrapplingHook = false;
                grapplingHookScript.TurnGrapHookOff();
            }

            if (!IsSliding)
            {
                StartCoroutine(RotateToZero());
            }
        }

        //in fixed update you should set all button click bools to false or they might go off twice
        //can't decide if should do same for holds; gives difference feel
        spaceDown = false;

        SUp = false;
        //SHold = false;

        Mouse0Up = false;
        Mouse0Down = false;

        //DHold = false;
        //AHold = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //stops play all player actions and x y movements, rotations, etc.
    public void LockPosition()
    {
        freezePlayer = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    //returns player movement
    public void UnLockPosition()
    {
        freezePlayer = false;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
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

    //gets closest point to on the edge that the player is next to
    private void GetContactPoint()
    {
        float closestPoint = 1000;
        for (int index = 0; index < currentEdge.pointCount; index++)
        {
            if (Vector2.Distance(currentEdge.points[index], transform.position) <= closestPoint)
            {
                closestPoint = Vector2.Distance(currentEdge.points[index], transform.position);
                currentEdgePoint = index;
            }
        }

        //get first location
        CurrentVector = new Vector2(currentEdge.points[currentEdgePoint].x, currentEdge.points[currentEdgePoint].y);

        //gets current perp vector with current and next point
        knownVec = CurrentVector - new Vector2(currentEdge.points[(currentEdgePoint + 1) % currentEdge.pointCount].x, currentEdge.points[(currentEdgePoint + 1) % currentEdge.pointCount].y);
        perpVec = CurrentVector + new Vector2(knownVec.y, -knownVec.x);
    }

    public IEnumerator RotateToZero()
    {
        if (!isRotating && !IsSliding && !isGrapplingHook)
        {
            isRotating = true;
            Quaternion start = transform.rotation;
            Quaternion end = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            float elapsed = 0.0f;
            while (elapsed < 1f)
            {
                //Debug.Log("rotate playercon");
                transform.rotation = Quaternion.Slerp(start, end, elapsed/.1f);
                elapsed += Time.deltaTime;
                if (Mathf.Abs(transform.rotation.eulerAngles.z) <= 5)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    elapsed = 1f;
                }

                yield return null;
            }
            isRotating = false;
        }
    }

    public void ResetPlayer()
    {
        //reset speed
        rb2d.velocity = Vector2.zero;

        //reset presses
        spaceDown = false;
        SHold = false;
        SUp = false;
        Mouse0Up = false;
        Mouse0Down = false;
        Mouse1Hold = false;
        DHold = false;
        AHold = false;

        //reset local variables

        //for grap hook
        isGrapplingHook = false;

        //for rotations
        isRotating = false;

        //for sliding skill
        IsSliding = false;
        firstRun = true;
        jumpOff = false;
        noMovement = false;
        //particles for sliding skill
        SlideParticles.Stop();
        SlideJumpParticles.Stop();

        //reset grappling hook;
        grapplingHookScript.TurnGrapHookOff();
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
        if (jumpOff)
        {
            if (collision.gameObject.GetComponent<EdgeCollider2D>())
            {
                currentEdge = collision.gameObject.GetComponent<EdgeCollider2D>();
                GetContactPoint();
                jumpOff = false;
                SlideJumpParticles.Stop();
            }
            else
            {
                //Debug.Log("no collider");
                //Debug.Log(transform.eulerAngles);
                //Debug.Log(transform.eulerAngles.z + 180f);
                ////Quaternion test = Quaternion.Euler(new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90f));
                ////transform.rotation = test;
                //transform.eulerAngles = new Vector3(0, 0, -(transform.eulerAngles.z - collision.transform.eulerAngles.z)/2 );
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
    }
}
