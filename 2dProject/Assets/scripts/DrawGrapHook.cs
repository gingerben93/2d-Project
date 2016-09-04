using UnityEngine;
using System.Collections;

public class DrawGrapHook : MonoBehaviour {

    bool drawHook = false;
    private float xPower { get; set; }
    private float yPower { get; set; }
    private float distEnd { get; set; }
    private float distCurrent { get; set; }
    private float playerDistToEnd { get; set; }

    private float currentDrawDistance { get; set; }
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

    private float yIncrement { get; set; }
    private float xIncrement { get; set; }

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
            playerDistToEnd = 1/0f;
        }

        currentPosPlayer = player.transform.position;
        if (distCurrent <= distEnd && drawHook == true)
        {
            
            MoveLine();
            /*
            distCurrent = Vector3.Distance(startPosLine, currentPosLine);
            currentPosLine.x += xIncrement;
            currentPosLine.y += yIncrement;
            line.SetPosition(1, currentPosLine);
            */
        }
        else
        {
            line.SetPosition(0, startPosLine);
            playerDistToEnd = Vector3.Distance(currentPosPlayer, endPosLine);
            //rb2d = player.GetComponent<Rigidbody2D>();
            //CalculateForceHook();
            //rb2d.AddForce(new Vector2(xPower, yPower));
        }

        //should turn off line, does nothing
        if (playerDistToEnd < 5f)
        {
            drawHook = false;
            line.enabled = false;
        }

	}

    void MoveLine()
    {
        currentDrawDistance += .3f / lineDrawSpeed;

        //linear interpolation to find a point between line
        float x = Mathf.Lerp(0, distEnd, currentDrawDistance);

        //normalize to give vector a magnitude of 1 then multple by currentDrawDistance for draw distance;
        currentPosLine = x * Vector3.Normalize(endPosLine - startPosLine) + startPosLine;

        distCurrent = Vector3.Distance(startPosLine, currentPosLine);

        line.SetPosition(0, currentPosPlayer);

        line.SetPosition(1, currentPosLine);
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
