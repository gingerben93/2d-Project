using UnityEngine;
using System.Collections;

public class DrawGrapHook : MonoBehaviour {

    bool drawHook = false;
    float xPower;
    float yPower;

    //reference to player
    public GameObject player;

    //for movement 
    private Rigidbody2D rb2d;

    private LineRenderer line;
    private Vector3 mousePos;
    private Vector3 startPos;
    private Vector3 endPos;

    //on start
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        line.SetVertexCount(2);
        line.SetWidth(.1f, .1f);
        line.SetColors(Color.green, Color.green);
        line.useWorldSpace = true;
    }

        // Update is called once per frame
        void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            startPos = player.transform.position;
            drawHook = true;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(0, startPos);
            line.SetPosition(1, mousePos);

            //startPos = mousePos;
        }
        /*
        else if (Input.GetMouseButtonUp(0))
        {
            if (line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                line.SetPosition(1, mousePos);
                endPos = mousePos;
                line = null;
            }
        }
        */

        if (drawHook == true)
        {
            startPos = player.transform.position;
            line.SetPosition(0, startPos);
            rb2d = player.GetComponent<Rigidbody2D>();
            CalculateForceHook();
            rb2d.AddForce(new Vector2(xPower, yPower));
        }

	}

    void CalculateForceHook()
    {
        //float dist = Vector3.Distance(startPos, mousePos);
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
}
