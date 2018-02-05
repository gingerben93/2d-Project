using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DrawGrapHook : MonoBehaviour
{

    private bool drawHook = false;
    
    private Vector2 startPosLine;
    private Vector2 endPosLine;

    //grappling hook tip
    public GameObject GrapTip;
    private BoxCollider2D grabTipCollider;
    private SpriteRenderer tipImage;
    public bool HasTipCollided { get; set; }
    public Rigidbody2D rb2dTip;

    //grap body
    private GameObject grapBody;
    private GameObject grap2Body;
    private BoxCollider2D grapBodyCollider;
    private BoxCollider2D grapBody2Collider;
    private SpriteRenderer bodyImage;
    private Vector2 difference;
    private float grapBodyAngle;
    private Quaternion bodyRoatation;
    public bool hasBodyCollided { get; set; }

    //bools for key press
    bool WDown, SDown;

    //joint
    public DistanceJoint2D joint;

    //player rotation
    private Quaternion playerRotation;

    //for hit moveing object
    public Transform enemyTransform;
    public bool hitEnemy;
    public int InstanceID;

    //on start
    void Start()
    {
        //grap hook info
        GrapTip = GameObject.Find("GrapplingHookTip");
        grabTipCollider = GrapTip.GetComponent<BoxCollider2D>();
        //change to false at somepoint
        HasTipCollided = false;
        rb2dTip = GrapTip.GetComponent<Rigidbody2D>();

        //grap hook body
        grapBody = gameObject.transform.parent.Find("BodyCollider").gameObject;
        grap2Body = gameObject.transform.parent.Find("BodyCollider2").gameObject;
        grapBodyCollider = grapBody.GetComponent<BoxCollider2D>();
        grapBody2Collider = grap2Body.GetComponent<BoxCollider2D>();
        bodyImage = grapBody.GetComponentInChildren<SpriteRenderer>();
        tipImage = GrapTip.GetComponentInChildren<SpriteRenderer>();
        //add joint
        joint = rb2dTip.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    void Update()
    {
        if (drawHook == true)
        {
            WDown = Input.GetKey(KeyCode.W);
            SDown = Input.GetKey(KeyCode.S);

            if (Vector2.Distance(PlayerController.PlayerControllerSingle.transform.position, GrapTip.transform.position) <= 1f)
            {
                WDown = false;
            }
            if (!HasTipCollided)
            {
                WDown = false;
            }

            if (hitEnemy)
            {
                GrapTip.transform.position = enemyTransform.position;
            }


            //for rotating body
            difference = GrapTip.transform.position - PlayerController.PlayerControllerSingle.transform.position;
            grapBody.transform.position = (PlayerController.PlayerControllerSingle.transform.position + GrapTip.transform.position) / 2;
            grap2Body.transform.position = (PlayerController.PlayerControllerSingle.transform.position + GrapTip.transform.position) / 2;

            grapBodyAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            bodyRoatation = Quaternion.AngleAxis(grapBodyAngle, Vector3.forward);
            grapBody.transform.rotation = bodyRoatation;
            grap2Body.transform.rotation = bodyRoatation;

            grapBodyCollider.size = new Vector2(Vector3.Distance(GrapTip.transform.position, PlayerController.PlayerControllerSingle.transform.position), .25f);
            grapBody2Collider.size = new Vector2(Vector3.Distance(GrapTip.transform.position, PlayerController.PlayerControllerSingle.transform.position), .25f);
            bodyImage.size = new Vector2(Vector3.Distance(GrapTip.transform.position, PlayerController.PlayerControllerSingle.transform.position), .2f);

        }
        else
        {
            GrapTip.transform.position = PlayerController.PlayerControllerSingle.transform.position;
        }
    }

    // fixedupdate gives correct collisons update doesn't. collides bad at fast speeds
    void FixedUpdate()
    {
        //needs to reset variables used for grap hook stuff
        if (drawHook == true)
        {
            // move up rope
            if (WDown)
            {
                joint.distance = Vector2.Distance(rb2dTip.transform.position, PlayerController.PlayerControllerSingle.transform.position);
                joint.enabled = false;
            }
            else if (!WDown && HasTipCollided)
            {

                joint.enabled = true;
            }

            //move down rope
            if (SDown)
            {
                joint.distance = Vector2.Distance(rb2dTip.transform.position, PlayerController.PlayerControllerSingle.transform.position);
                joint.enabled = false;
            }

            else if (!SDown && HasTipCollided)
            {
                joint.enabled = true;
            }

            if (!HasTipCollided)
            {
                //now moves in grap tip
                GrapTip.transform.position += GrapTip.transform.right * .5f;
                //rb2dTip.velocity = GrapTip.transform.right * 15f;
                //rb2dTip.AddForce(GrapTip.transform.right * .5f);
            }
            else
            {
                if (PlayerController.PlayerControllerSingle.isGrapplingHook && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
                {
                    PlayerController.PlayerControllerSingle.rb2d.gravityScale = 0;
                }
                else
                {
                    PlayerController.PlayerControllerSingle.rb2d.gravityScale = 1;
                }
                //don't rotate when sliding or move when sliding
                if (!PlayerController.PlayerControllerSingle.IsSliding)
                {
                    //for rotating the player object;
                    playerRotation = Quaternion.LookRotation(Vector3.forward, GrapTip.transform.position - PlayerController.PlayerControllerSingle.transform.position);
                    PlayerController.PlayerControllerSingle.transform.rotation = Quaternion.Slerp(PlayerController.PlayerControllerSingle.transform.rotation, playerRotation, .2f);

                    if (WDown)
                    {
                        //PlayerController.PlayerControllerSingle.transform.position += PlayerController.PlayerControllerSingle.transform.up * .5f;
                        if (PlayerController.PlayerControllerSingle.rb2d.velocity.y <= 10f)
                        {
                            PlayerController.PlayerControllerSingle.rb2d.velocity += (Vector2)PlayerController.PlayerControllerSingle.transform.up * .5f;
                        }
                    }
                }
            }
        }
    }

    public void TurnGrapHookOn()
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            //grap body info
            grapBody.transform.position = PlayerController.PlayerControllerSingle.transform.position;
            grap2Body.transform.position = PlayerController.PlayerControllerSingle.transform.position;
            grapBodyCollider.enabled = true;
            grapBody2Collider.enabled = true;
            grapBody.GetComponent<Rigidbody2D>().isKinematic = false;
            bodyImage.enabled = true;

            //grap tip info
            grabTipCollider.enabled = true;
            GrapTip.transform.position = PlayerController.PlayerControllerSingle.transform.position;
            //set tip to dynamic for collision detection
            rb2dTip.isKinematic = false;
            tipImage.enabled = true;
            HasTipCollided = false;

            startPosLine = PlayerController.PlayerControllerSingle.transform.position;
            endPosLine = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float TipAngle = Mathf.Atan2(endPosLine.y - startPosLine.y, endPosLine.x - startPosLine.x) * Mathf.Rad2Deg;
            Quaternion tipRotation = Quaternion.AngleAxis(TipAngle, Vector3.forward);
            GrapTip.transform.rotation = tipRotation;

            //start grap hook
            drawHook = true;
        }
    }

    public void TurnGrapHookOff()
    {
        //HasTipCollided = true;

        //set to kinmatic so it doens't move while it's off
        grapBody.GetComponent<Rigidbody2D>().isKinematic = true;

        //body stuff
        grapBodyCollider.size = new Vector2(.5f, .5f);
        grapBody2Collider.size = new Vector2(.5f, .5f);
        bodyImage.size = new Vector2(.1f, .2f);
        grapBodyCollider.enabled = false;
        grapBody2Collider.enabled = false;
        bodyImage.enabled = false;


        //tip stuff
        rb2dTip.isKinematic = true;
        tipImage.enabled = false;
        grabTipCollider.enabled = false;

        // for resetting pulling stuff
        WDown = false;
        SDown = false;

        joint.enabled = false;
        //turn off tip
        drawHook = false;
    }
}
