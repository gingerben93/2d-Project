using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public bool facingRight { get; set; }
    public bool jump { get; set; }

    public float moveForce { get; set; }
    public float maxSpeed{ get; set; }
    public float jumpForce{ get; set; }
    public Transform groundCheck;

    //private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    DoorCollider doorInfo;

    //Inventory
    public CanvasGroup InvMenu;

    //is touching door
    public bool touchingDoor { get; set; }
    public string mapSeed { get; set; }
    public int doorRef { get; set; }

    //Inventory
    public Inventory inventory;

    //getting map generator
    MapGenerator map;

    // Use this for initialization
    void Start () {
        //variables on start
        maxSpeed = 5f;
        moveForce = 5f;
        jumpForce = 500;
        touchingDoor = false;
        facingRight = true;
        jump = false;

        //other start stuff
        doorInfo = FindObjectOfType<DoorCollider>();
        rb2d = GetComponent<Rigidbody2D>();
        map = FindObjectOfType<MapGenerator>();
        Vector2 door;
        door = map.doorLocations[doorRef];
        transform.position = new Vector3(door.x, door.y, 0);
    }

	
	// Update is called once per frame
	void Update () {
        
        Resources.UnloadUnusedAssets();
        if (Input.GetKeyDown(KeyCode.R) && touchingDoor)
        {
            map = FindObjectOfType<MapGenerator>();
            doorInfo = FindObjectOfType<DoorCollider>();

            map.seed = mapSeed;
            map.LoadMap();

            Vector2 door;
            door = map.doorLocations[doorInfo.numVal];
            transform.position = new Vector3(door.x, door.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb2d.velocity.y < maxSpeed /*&& grounded*/)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            inventory.AddItem(collision.GetComponent<Item>());
            Destroy(collision.gameObject);
        }
    }

}
