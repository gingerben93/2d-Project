using UnityEngine;
using System.Collections;

public class DrawGrapHook : MonoBehaviour {

    private bool drawHook = false;
    private bool moveUpRope = false;
    private bool moveDownRope = false;

    private float distEnd;
    private float distCurrent;
    private float playerDistToEnd;
    //private float currentRopeLength;


    private float currentDrawDistance;
    //private float currentPlayerDistance;
    private float lineDrawSpeed = .1f;

    //reference to player
    public GameObject player;

    //for movement 
    private Rigidbody2D rb2d;

    private LineRenderer line;
    private Vector3 mousePos;
    private Vector3 currentPosLine;
    private Vector3 startPosLine;
    private Vector3 endPosLine;
    private Vector3 currentPosPlayer;

    private float yIncrement;
    private float xIncrement;

    //for moving player up or down rope
    private Vector3 heading;
    private float distance;
    private Vector3 direction;

    private Vector3 StartDirection;

    //grappling hook tip
    private GameObject GrapTip;
    public bool HasTipCollided;
    private Rigidbody2D rb2dTip;
    private Vector3 TipDirection;
    private float angleTipCollider;

    //grab hook joint
    //DistanceJoint2D joint;


    //on start
    void Start()
    {
        //line info
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        line.SetVertexCount(2);
        line.SetWidth(.1f, .1f);
        line.SetColors(Color.green, Color.green);
        line.useWorldSpace = true;
        line.enabled = false;
        rb2d = player.GetComponent<Rigidbody2D>();

        //grap hook info
        GrapTip = GameObject.Find("GrapplingHookTip");
        HasTipCollided = true;
        //joint = GrapTip.GetComponent<DistanceJoint2D>();
        //joint.connectedAnchor = Vector2.zero;
    }

        // Update is called once per frame
    void Update ()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //grap tip info
            GrapTip.GetComponent<BoxCollider2D>().enabled = true;
            rb2dTip = GrapTip.GetComponent<Rigidbody2D>();
            HasTipCollided = true;
            //joint.enabled = true;
            

            //turn gravity back on
            rb2d.gravityScale = 1;
            moveUpRope = false;
            moveDownRope = false;

            startPosLine = player.transform.position;
            drawHook = true;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            currentPosLine = startPosLine;
            endPosLine = mousePos;

            //CalculateCurrentDrawPosition();
            distEnd = Vector3.Distance(startPosLine, endPosLine);

            line.SetPosition(0, startPosLine);
            line.SetPosition(1, startPosLine);
            line.enabled = true;

            //reset variable in MoveLine
            currentDrawDistance = 0;
            distCurrent = 0;

            //info grap hook tip
            GrapTip.transform.position = currentPosPlayer;
            heading = endPosLine - startPosLine;
            distance = heading.magnitude;
            TipDirection = heading / distance;

        }

        currentPosPlayer = player.transform.position;
        //GrapTip.transform.position = currentPosPlayer;
        if (drawHook == true)
        {
            angleTipCollider = Vector2.Angle(currentPosPlayer, GrapTip.transform.position);
            GrapTip.transform.eulerAngles = new Vector3(0, 0, angleTipCollider);
            //GrapTip.transform.position = currentPosLine;
            // move up rope
            if (Input.GetKeyDown(KeyCode.W))
            {
                heading = endPosLine - startPosLine;
                distance = heading.magnitude;
                StartDirection = heading / distance;

                moveUpRope = true;
            }

            else if (Input.GetKeyUp(KeyCode.W))
            {
                rb2d.gravityScale = 1;
                //currentPlayerDistance = 0;
                moveUpRope = false;
            }

            //move down rope
            if (Input.GetKeyDown(KeyCode.S))
            {
                heading = endPosLine - startPosLine;
                distance = heading.magnitude;
                StartDirection = heading / distance;

                moveDownRope = true;
            }

            else if (Input.GetKeyUp(KeyCode.S))
            {
                rb2d.gravityScale = 1;
                //currentPlayerDistance = 0;
                moveDownRope = false;
            }

            
            //draw the rope
            if (HasTipCollided)
            {
                rb2dTip.velocity = new Vector2(10 * TipDirection.x, 10 * TipDirection.y);
                MoveLine();
                playerDistToEnd = Vector3.Distance(currentPosPlayer, currentPosLine);
                /*
                distCurrent = Vector3.Distance(startPosLine, currentPosLine);
                currentPosLine.x += xIncrement;
                currentPosLine.y += yIncrement;
                line.SetPosition(1, currentPosLine);
                */
            }
            else {
                /*
                joint.anchor = Vector2.zero;
                joint.connectedAnchor = currentPosPlayer - currentPosLine;
                joint.distance = playerDistToEnd;
                */
                line.SetPosition(0, currentPosPlayer);

                //jump to turn off rope
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //joint.enabled = false;
                    distCurrent = 0;
                    drawHook = false;
                    line.enabled = false;
                    GrapTip.GetComponent<BoxCollider2D>().enabled = false;
                    GrapTip.transform.position = currentPosPlayer;
                }

                if (moveUpRope == true)
                {
                    rb2d.gravityScale = 0;
                    playerDistToEnd = Vector3.Distance(currentPosPlayer, currentPosLine);
                    MovePlayerUpRope();
                }
                else
                {
                    //joint.anchor = currentPosPlayer;
                }

                if (moveDownRope == true)
                {
                    rb2d.gravityScale = 0;
                    playerDistToEnd = Vector3.Distance(currentPosPlayer, currentPosLine);
                    MovePlayerDownRope();
                }
            }
        }
    }

    void MoveLine()
    {
        /*
        currentDrawDistance += .5f / lineDrawSpeed;

        //linear interpolation to find a point between line
        float x = Mathf.Lerp(0, distEnd, currentDrawDistance);

        //normalize to give vector a magnitude of 1 then multple by currentDrawDistance for draw distance;
        currentPosLine = x * Vector3.Normalize(endPosLine - startPosLine) + startPosLine;

        //get current distance from start to hook (rope length)
        distCurrent = Vector3.Distance(startPosLine, currentPosLine);
        */

        //currentRopeLength = Vector3.Distance(currentPosPlayer, currentPosLine);
        currentDrawDistance = lineDrawSpeed;



        heading = endPosLine - startPosLine;

        distance = heading.magnitude;
        StartDirection = heading / distance;

        currentPosLine += StartDirection * currentDrawDistance;

        //distCurrent = Vector3.Distance(startPosLine, currentPosLine);


        //set line coordinates
        currentPosLine = GrapTip.transform.position;
        line.SetPosition(0, currentPosPlayer);
        line.SetPosition(1, currentPosLine);
    }

    void MovePlayerUpRope()
    {
        heading = currentPosLine - currentPosPlayer;

        distance = heading.magnitude;
        direction = heading / distance;

        if (StartDirection.x != direction.x)
        {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
            rb2d.AddForce(new Vector2(0, direction.y * 500f));
        }
        else
        {
            rb2d.AddForce(new Vector2(direction.x * 500f, 0));
        }

        if (StartDirection.y != direction.y)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
            rb2d.AddForce(new Vector2(direction.x * 500f, 0));
        }
        else
        {
            rb2d.AddForce(new Vector2(0, direction.y * 500f));
        }

        //rb2d.AddForce(new Vector2(direction.x * 500f, direction.y * 10f));

        /*
        currentPlayerDistance += .5f / lineDrawSpeed;
        float x = Mathf.Lerp(0, playerDistToEnd, currentPlayerDistance);
        currentPosLine = x * Vector3.Normalize(endPosLine - currentPosPlayer) + currentPosPlayer;

        player.transform.position = currentPosLine;
        */
    }

    void MovePlayerDownRope()
    {
        heading = currentPosLine - currentPosPlayer;

        distance = heading.magnitude;
        direction = heading / distance;

        if (StartDirection.x != direction.x)
        {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
            rb2d.AddForce(new Vector2(0, -direction.y * 500f));
        }
        else
        {
            rb2d.AddForce(new Vector2(-direction.x * 500f, 0));
        }

        if (StartDirection.y != direction.y)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
            rb2d.AddForce(new Vector2(-direction.x * 500f, 0));
        }
        else
        {
            rb2d.AddForce(new Vector2(0, -direction.y * 500f));
        }
    }   
}
