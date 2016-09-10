using UnityEngine;
using System.Collections;

public class DrawGrapHook : MonoBehaviour {

    private bool drawHook = false;
    private bool moveUpRope = false;
    private bool moveDownRope = false;

    private float xPower;
    private float yPower;
    private float distEnd;
    private float distCurrent;
    private float playerDistToEnd;
    private float currentRopeLength;


    private float currentDrawDistance;
    private float currentPlayerDistance;
    private float lineDrawSpeed = 20f;

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

    //on start
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        line.SetVertexCount(2);
        line.SetWidth(.1f, .1f);
        line.SetColors(Color.green, Color.green);
        line.useWorldSpace = true;
        line.enabled = false;
        rb2d = player.GetComponent<Rigidbody2D>();
    }

        // Update is called once per frame
        void Update () {

        if (Input.GetMouseButtonDown(0))
        {
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
        }

        currentPosPlayer = player.transform.position;
        if (drawHook == true)
        {
            // move up rope
            if (Input.GetKeyDown(KeyCode.W))
            {
                rb2d.velocity = new Vector3(0, 0, 0);
                moveUpRope = true;
            }

            else if (Input.GetKeyUp(KeyCode.W))
            {
                rb2d.gravityScale = 1;
                currentPlayerDistance = 0;
                moveUpRope = false;
            }

            //move down rope
            if (Input.GetKeyDown(KeyCode.S))
            {
                rb2d.velocity = new Vector3(0, 0, 0);
                rb2d.gravityScale = 0;
                moveDownRope = true;
            }

            else if (Input.GetKeyUp(KeyCode.S))
            {
                rb2d.gravityScale = 1;
                currentPlayerDistance = 0;
                moveDownRope = false;
            }

            //draw the rope
            if (distCurrent < distEnd)
            {
                MoveLine();
                /*
                distCurrent = Vector3.Distance(startPosLine, currentPosLine);
                currentPosLine.x += xIncrement;
                currentPosLine.y += yIncrement;
                line.SetPosition(1, currentPosLine);
                */
            }
            else {
                line.SetPosition(0, currentPosPlayer);
                //rb2d = player.GetComponent<Rigidbody2D>();
                //CalculateForceHook();
                //rb2d.AddForce(new Vector2(xPower, yPower));


                /*
                if (distEnd <= distCurrent)
                {
                    rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
                }
                */

                //jump to turn off rope
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    distCurrent = 0;
                    drawHook = false;
                    line.enabled = false;
                }

                if (moveUpRope == true)
                {
                    rb2d.gravityScale = 0;
                    playerDistToEnd = Vector3.Distance(currentPosPlayer, endPosLine);
                    MovePlayerUpRope();
                }

                if (moveDownRope == true)
                {
                    rb2d.gravityScale = 0;
                    playerDistToEnd = Vector3.Distance(currentPosPlayer, endPosLine);
                    MovePlayerDownRope();
                }
            }
        }

    }

    void MoveLine()
    {
        currentDrawDistance += .5f / lineDrawSpeed;

        //linear interpolation to find a point between line
        float x = Mathf.Lerp(0, distEnd, currentDrawDistance);

        //normalize to give vector a magnitude of 1 then multple by currentDrawDistance for draw distance;
        currentPosLine = x * Vector3.Normalize(endPosLine - startPosLine) + startPosLine;

        //get current distance from start to hook (rope length)
        distCurrent = Vector3.Distance(startPosLine, currentPosLine);

        currentRopeLength = Vector3.Distance(currentPosPlayer, currentPosLine);


        //set line coordinates
        line.SetPosition(0, currentPosPlayer);
        line.SetPosition(1, currentPosLine);
    }

    void MovePlayerUpRope()
    {
        Vector3 heading = endPosLine - currentPosPlayer;

        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        rb2d.AddForce(new Vector2(direction.x * 500f, direction.y * 10f));
        /*
        currentPlayerDistance += .5f / lineDrawSpeed;
        float x = Mathf.Lerp(0, playerDistToEnd, currentPlayerDistance);
        currentPosLine = x * Vector3.Normalize(endPosLine - currentPosPlayer) + currentPosPlayer;

        player.transform.position = currentPosLine;
        */
    }

    void MovePlayerDownRope()
    {
        Vector3 heading = endPosLine - currentPosPlayer;

        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        rb2d.AddForce(new Vector2(-direction.x * 500f, -direction.y * 10f));
    }


        /*
        void CalculateForceHook()
        {
            distEnd = Vector3.Distance(startPosLine, endPosLine);
            if (player.transform.position.x < mousePos.x)
            {
                xPower = 500;
            }
            if (player.transform.position.x > mousePos.x)
            {
                xPower = -500;
            }
            if (player.transform.position.y < mousePos.y)
            {
                yPower = 50;
            }
            if (player.transform.position.y > mousePos.y)
            {
                yPower = -50;
            }

        }

        void CalculateCurrentDrawPosition()
        {
            //x distance between vectors
            if(startPosLine.x <= 0)
            {
                if(endPosLine.x >= 0)
                {
                    Mathf.Abs(xIncrement = endPosLine.x - startPosLine.x);
                    xIncrement *= -1;
                }
                if (endPosLine.x < 0)
                {
                    Mathf.Abs(xIncrement = endPosLine.x + startPosLine.x);

                }
            }
            if (startPosLine.x > 0)
            {
                if (endPosLine.x >= 0)
                {
                    Mathf.Abs(xIncrement = endPosLine.x + startPosLine.x);
                }
                if (endPosLine.x < 0)
                {
                    Mathf.Abs(xIncrement = endPosLine.x - startPosLine.x);
                    xIncrement *= -1;
                }
            }

            //y distance between vectors
            if (startPosLine.y <= 0)
            {
                if (endPosLine.y >= 0)
                {
                    Mathf.Abs(yIncrement = endPosLine.y - startPosLine.y);
                }
                if (endPosLine.y < 0)
                {
                    Mathf.Abs(yIncrement = endPosLine.y + startPosLine.y);
                    yIncrement *= -1;
                }
            }
            if (startPosLine.y > 0)
            {
                if (endPosLine.y >= 0)
                {
                    Mathf.Abs(yIncrement = endPosLine.y + startPosLine.y);
                }
                if (endPosLine.y < 0)
                {
                    Mathf.Abs(yIncrement = endPosLine.y - startPosLine.y);
                    yIncrement *= -1;
                }
            }
            xIncrement /= 500;
            yIncrement /= 500;

            Debug.Log("xIncrement = " + xIncrement);
            Debug.Log("yIncrement = " + yIncrement);
        }
        */
    }
