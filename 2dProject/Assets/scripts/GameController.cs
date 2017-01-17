using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float moveForce { get; set; }
    public float maxSpeed = 500f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    //private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    //Inventory
    public CanvasGroup InvMenu;

    //is touching door
    public bool touchingDoor { get; set; }

    public string mapSeed { get; set; }
    public int doorRef { get; set; }

    // Use this for initialization
    void Start () {
        //variables on start
        moveForce = 365f;
        touchingDoor = false;

        //other start stuff

        rb2d = GetComponent<Rigidbody2D>();
        MapGenerator map = FindObjectOfType<MapGenerator>();
        Vector2 door;
        door = map.doorLocations[doorRef];
        transform.position = new Vector3(door.x, door.y, 0);
    }

	
	// Update is called once per frame
	void Update () {
        
        Resources.UnloadUnusedAssets();
        if (Input.GetKeyDown(KeyCode.R) && touchingDoor)
        {
            MapGenerator map = FindObjectOfType<MapGenerator>();
            DoorCollider doorInfo = FindObjectOfType<DoorCollider>();
            DrawPlayerMap changeLocalMap = FindObjectOfType<DrawPlayerMap>();

            map.seed = mapSeed;
            map.GenerateMap();
            changeLocalMap.touchingDoor = true;

            Vector2 door;
            door = map.doorLocations[doorInfo.numVal];
            transform.position = new Vector3(door.x, door.y, 0);


        }

        if (Input.GetKeyDown(KeyCode.Space) /*&& grounded*/)
        {
            jump = true;
            rb2d.gravityScale = 1;
        }

         // 5 - Shooting
         bool shoot = Input.GetMouseButtonDown(1);
         shoot |= Input.GetMouseButtonDown(1);
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            InvMenu.alpha = (InvMenu.alpha + 1) % 2;
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
            rb2d.gravityScale = 1;
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector3(0, 0, 0);
            }
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
