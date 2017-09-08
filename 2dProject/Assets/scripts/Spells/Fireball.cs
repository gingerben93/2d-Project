using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    public GameObject player;

    private Vector3 mousePos;
    private Vector3 heading;
    private float distance;
    private Vector3 direction;

    //arc curve variables
    float angle;
    float angleX;
    float angleY;

    private Vector2 movement;
    public Rigidbody2D rigidbodyComponent;
    // Update is called once per frame

    void Start()
    {
        Shoot();
        Destroy(gameObject, 5);
        transform.position = GameObject.Find("Hero").transform.position;
    }

    void FixedUpdate()
    {

    }

    void Shoot()
    {
        //temp = Input.mousePosition;
        var temp = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(temp);
        mousePos.z = 0;

        heading = mousePos - GameController.GameControllerSingle.transform.position;

        float vel, x, y, g, angle1, angle2;
        vel = 5;
        x = heading.x;
        y = heading.y;
        g = -9.81f;

        rigidbodyComponent.velocity = new Vector2(heading.x * vel, heading.y * vel);

        //angle1 = Mathf.Atan((Mathf.Pow(vel,2) + Mathf.Sqrt(Mathf.Pow(vel,4) - g*(g*x + 2*y*Mathf.Pow(vel,2)))) / (g*x) );
        //angle2 = Mathf.Atan((Mathf.Pow(vel, 2) - Mathf.Sqrt(Mathf.Pow(vel, 4) - g * (g * x + 2 * y * Mathf.Pow(vel, 2)))) / (g * x));

        //Debug.Log(heading);
        //Debug.Log(angle1 * (180 / Mathf.PI));
        //Debug.Log(angle2 * (180 / Mathf.PI));

        //angleX = Mathf.Atan(((Mathf.Pow(10, 2)) + Mathf.Sqrt(Mathf.Pow(10, 4) - -9.81f * (-9.81f * (Mathf.Pow(heading.x, 2) + (2 * heading.y * (Mathf.Pow(10, 2))))))) / (-9.81f * heading.x));
        //angleY = Mathf.Atan(((Mathf.Pow(10, 2)) - Mathf.Sqrt(Mathf.Pow(10, 4) - -9.81f * (-9.81f * (Mathf.Pow(heading.x, 2) + (2 * heading.y * (Mathf.Pow(10, 2))))))) / (-9.81f * heading.x));

        //Debug.Log(angle);
        //Debug.Log(angleX * (180 / Mathf.PI));
        //Debug.Log(angleY * (180 / Mathf.PI));


        //angle = Vector2.Angle(mousePos, GameController.GameControllerSingle.transform.position);
        //Debug.Log(GameController.GameControllerSingle.transform.position);
        //Debug.Log(mousePos);
        //Debug.Log(angle);

        //if (angle1 * (180 / Mathf.PI) >= 0)
        //{
        //    if (angle2 * (180 / Mathf.PI) >= 0)
        //    {
        //        Debug.Log("quad 3");
        //    }
        //    else
        //    {
        //        Debug.Log("quad 2");
        //    }
        //}
        //else
        //{
        //    if (angle2 * (180 / Mathf.PI) >= 0)
        //    {
        //        Debug.Log("quad 1");
        //        rigidbodyComponent.velocity = new Vector2(Mathf.Cos(angle2) * vel, Mathf.Sin(angle2) * vel);

        //    }
        //    else
        //    {
        //        Debug.Log("quad 4");
        //    }
        //}
    }

}
