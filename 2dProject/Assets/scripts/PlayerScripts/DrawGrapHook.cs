using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DrawGrapHook : MonoBehaviour
{

    private bool drawHook = false;
    private bool moveUpRope = false;
    private bool moveDownRope = false;

    //private float currentDrawDistance;
    private float lineDrawSpeed = .1f;

    //reference to player
    private GameObject player;

    //for movement 
    private Rigidbody2D rb2d;

    public LineRenderer line;
    private Vector2 mousePos;
    public Vector2 currentPosLine;
    private Vector2 startPosLine;
    private Vector2 endPosLine;
    private Vector2 currentPosPlayer;

    private float yIncrement;
    private float xIncrement;

    //for moving player up or down rope
    private Vector2 heading;
    private float distance;
    private Vector2 direction;

    private Vector2 StartDirection;

    //grappling hook tip
    private GameObject GrapTip;
    public bool HasTipCollided { get; set; }
    public Rigidbody2D rb2dTip;
    private Vector2 TipDirection;
    private float angleTipCollider;

    //grap body
    private GameObject grapBody;
    private Vector2 difference;
    private float distanceInX;
    private float distanceInY;
    private float grapBodyAngle;
    private Quaternion q;
    public bool hasBodyCollided { get; set; }

    //grab hook pull power
    //float grapplingPullSpeed = 25f;
    //float hookMaxSpeed = 10f;

    //number of lines
    public int currentNumberLines = 2;

    //bools for key press
    bool WDown, SDown;

    //joint
    public DistanceJoint2D joint;

    //on start
    void Start()
    {
        //line info
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        //line.SetVertexCount(2);
        //line.SetWidth(.1f, .1f);
        //line.SetColors(Color.green, Color.green);

        line.numPositions = 2;
        line.startWidth = .1f;
        line.endWidth = .1f;
        line.startColor = Color.green;
        line.endColor = Color.green;

        player = GameController.GameControllerSingle.gameObject;

        line.useWorldSpace = true;
        line.enabled = false;
        rb2d = player.GetComponent<Rigidbody2D>();

        //grap hook info
        GrapTip = GameObject.Find("GrapplingHookTip");
        //change to false at somepoint
        HasTipCollided = false;
        rb2dTip = GrapTip.GetComponent<Rigidbody2D>();

        //grap hook body
        grapBody = GameObject.Find("BodyCollider");

        //add joint
        joint = rb2dTip.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                //turn off joint
                joint.enabled = false;

                //rest number lines
                currentNumberLines = 2;
                line.numPositions = currentNumberLines;

                //set tip to dynamic for collision detection
                rb2dTip.isKinematic = false;
                //rb2dTip.simulated = true;

                //SET BODY INFO
                //grap body info
                grapBody.GetComponent<BoxCollider2D>().enabled = true;
                grapBody.GetComponent<BoxCollider2D>().size = new Vector2(.5f, .5f);
                grapBody.GetComponent<Rigidbody2D>().isKinematic = false;
                grapBody.GetComponent<Rigidbody2D>().simulated = true;

                //grap tip info
                GrapTip.GetComponent<BoxCollider2D>().enabled = true;

                HasTipCollided = false;

                //turn gravity back on
                rb2d.gravityScale = 1;
                moveUpRope = false;
                moveDownRope = false;

                startPosLine = player.transform.position;
                drawHook = true;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //mousePos.z = 0;

                currentPosLine = startPosLine;
                endPosLine = mousePos;

                line.SetPosition(currentNumberLines - 2, startPosLine);
                line.SetPosition(currentNumberLines - 1, startPosLine);
                line.enabled = true;

                //reset variable in MoveLine
                //currentDrawDistance = 0;

                //info grap hook tip
                GrapTip.transform.position = currentPosPlayer;
                heading = endPosLine - startPosLine;
                distance = heading.magnitude;
                TipDirection = heading / distance;
            }
        }

        if (drawHook == true)
        {
            WDown = Input.GetKey(KeyCode.W);
            SDown = Input.GetKey(KeyCode.S);
        }

        currentPosPlayer = player.transform.position;
    }

    // fixedupdate gives correct collisons update doesn't. collides bad at fast speeds
    void FixedUpdate()
    {
        //needs to reset variables used for grap hook stuff
        if (drawHook == true)
        {
            //angleTipCollider = Vector3.Angle(currentPosPlayer, mousePos);
            //GrapTip.transform.eulerAngles = new Vector3(0, 0, angleTipCollider);

            // move up rope
            if (WDown && !moveUpRope)
            {
                //calculate direction
                //heading = endPosLine - startPosLine;
                //distance = heading.magnitude;
                //StartDirection = heading / distance;
                moveUpRope = true;
                joint.enabled = false;
            }

            else if (!WDown && moveUpRope)
            {
                rb2d.gravityScale = 1;
                moveUpRope = false;
                joint.distance = Vector2.Distance(rb2dTip.transform.position, GameController.GameControllerSingle.transform.position);
                joint.enabled = true;
            }

            //move down rope
            if (SDown && !moveDownRope)
            {
                //calculate direction
                //heading = endPosLine - startPosLine;
                //distance = heading.magnitude;
                //StartDirection = heading / distance;
                moveDownRope = true;
                joint.enabled = false;
            }

            else if (!SDown && moveDownRope)
            {
                rb2d.gravityScale = 1;
                moveDownRope = false;
                joint.distance = Vector2.Distance(rb2dTip.transform.position, GameController.GameControllerSingle.transform.position);
                joint.enabled = true;
            }

            if (!HasTipCollided)
            {
                rb2dTip.MovePosition(rb2dTip.position + new Vector2(.4f * TipDirection.x, .4f * TipDirection.y));
                MoveLine();
            }
            else
            {
                line.SetPosition(currentNumberLines - 1, currentPosPlayer);

                //jump to turn off rope
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    drawHook = false;
                    line.enabled = false;
                    GrapTip.GetComponent<BoxCollider2D>().enabled = false;
                    grapBody.GetComponent<BoxCollider2D>().enabled = false;
                    GrapTip.transform.position = currentPosPlayer;

                    //turn off tip
                    joint.enabled = false;
                }

                if (moveUpRope == true)
                {
                    rb2d.gravityScale = 0;
                    rb2d.position = Vector3.MoveTowards(rb2d.position, GrapTip.transform.position, .25f);
                }

                //if (moveDownRope == true)
                //{
                //    //calculate direction for pushing away
                //    currentPosPlayer = GameController.GameControllerSingle.transform.position;
                //    heading = currentPosLine - currentPosPlayer;
                //    rb2d.gravityScale = 0;
                //    rb2d.position = Vector2.MoveTowards(rb2d.position, rb2d.position - heading, .25f);
                //    //MovePlayerDownRope();
                //}
            }

            //for rotating body
            difference = GrapTip.transform.position - player.transform.position;
            distanceInX = difference.x;
            distanceInY = difference.y;

            grapBodyAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            grapBody.transform.position = GrapTip.transform.position - new Vector3(distanceInX / 2f, distanceInY / 2f, 0);
            q = Quaternion.AngleAxis(grapBodyAngle, Vector3.forward);
            grapBody.transform.rotation = q;
            grapBody.GetComponent<BoxCollider2D>().size = new Vector2(Vector3.Distance(GrapTip.transform.position, player.transform.position) * 4, .5f);
        }
    }

    void MoveLine()
    {
        //currentDrawDistance = lineDrawSpeed;

        //calculate vector direction
        heading = endPosLine - startPosLine;
        distance = heading.magnitude;
        StartDirection = heading / distance;

        currentPosLine += StartDirection * lineDrawSpeed;

        //set line coordinates
        currentPosLine = GrapTip.transform.position;
        line.SetPosition(currentNumberLines - 1, currentPosPlayer);
        //off set is to make it appeat closer to wall ; + StartDirection * .5f
        line.SetPosition(currentNumberLines - 2, currentPosLine + StartDirection * .5f);
    }

    //void MovePlayerUpRope()
    //{
    //    Vector2 temp = rb2dTip.transform.InverseTransformDirection(rb2d.transform.position);
    //    Debug.Log(temp);
    //    if (temp.x <= hookMaxSpeed)
    //    {
    //        heading = currentPosLine - currentPosPlayer;

    //        distance = heading.magnitude;
    //        direction = heading / distance;

    //        rb2d.AddForce(new Vector2(direction.x * grapplingPullSpeed, 0));
    //    }
    //    else
    //    {
    //        Debug.Log("MAX X");
    //    }
    //    if (temp.y <= hookMaxSpeed)
    //    {
    //        heading = currentPosLine - currentPosPlayer;
    //        distance = heading.magnitude;
    //        direction = heading / distance;

    //        rb2d.AddForce(new Vector2(0, direction.y * grapplingPullSpeed));
    //    }
    //    else
    //    {
    //        Debug.Log("MAX Y");
    //    }

    //    //if (StartDirection.x != direction.x)
    //    //{
    //    //    //rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
    //    //    rb2d.AddForce(new Vector2(0, direction.y * grapplingPullSpeed));
    //    //}
    //    //else
    //    //{
    //    //    rb2d.AddForce(new Vector2(direction.x * grapplingPullSpeed, 0));
    //    //}

    //    //if (StartDirection.y != direction.y)
    //    //{
    //    //    //rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
    //    //    rb2d.AddForce(new Vector2(direction.x * grapplingPullSpeed, 0));
    //    //}
    //    //else
    //    //{
    //    //    rb2d.AddForce(new Vector2(0, direction.y * grapplingPullSpeed));
    //    //}
    //}

    //void MovePlayerDownRope()
    //{
    //    Vector2 temp = rb2dTip.transform.InverseTransformDirection(rb2d.transform.position);
    //    Debug.Log(temp);

    //    if (temp.x >= -hookMaxSpeed)
    //    {
    //        //calculate direction
    //        heading = currentPosLine - currentPosPlayer;
    //        distance = heading.magnitude;
    //        direction = heading / distance;

    //        rb2d.AddForce(new Vector2(-direction.x * grapplingPullSpeed, 0));
    //    }
    //    else
    //    {
    //        Debug.Log("-MAX X");
    //    }

    //    if (temp.y >= -hookMaxSpeed)
    //    {
    //        rb2d.AddForce(new Vector2(0, -direction.y * grapplingPullSpeed));
    //    }
    //    else
    //    {
    //        Debug.Log("-MAX Y");
    //    }

    //    //if (StartDirection.x != direction.x)
    //    //{
    //    //    //rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
    //    //    rb2d.AddForce(new Vector2(0, -direction.y * grapplingPullSpeed));
    //    //}
    //    //else
    //    //{
    //    //    rb2d.AddForce(new Vector2(-direction.x * grapplingPullSpeed, 0));
    //    //}

    //    //if (StartDirection.y != direction.y)
    //    //{
    //    //    //rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
    //    //    rb2d.AddForce(new Vector2(-direction.x * grapplingPullSpeed, 0));
    //    //}
    //    //else
    //    //{
    //    //    rb2d.AddForce(new Vector2(0, -direction.y * grapplingPullSpeed));
    //    //}
    //}
}
