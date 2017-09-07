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
        Destroy(gameObject, 2);
        transform.position = GameObject.Find("Hero").transform.position;

    }

    void FixedUpdate()
    {
        //if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        //rigidbodyComponent.velocity = movement;
    }

    void Shoot()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        heading = mousePos - GameController.GameControllerSingle.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        movement = new Vector2(10 * direction.x, 10 * direction.y);

        //627.84 corralates to the gravity scale of 6.2784 on the rigidbody. I dont get it, but it works?? kinda?? unintentionally???
        //velocity is 10k but i have to add a force of 1250 to make it work. Def something wrong.
        angle = (180 / Mathf.PI) * (Mathf.Asin(627.84f * heading.x / 10000.0f));

        //angleX = Mathf.Atan(((Mathf.Pow(100, 2)) + Mathf.Sqrt(Mathf.Pow(100, 4) - -9.81f * (-9.81f * (Mathf.Pow(Mathf.Abs(heading.x), 2) + (2 * Mathf.Abs(heading.y) * (Mathf.Pow(100, 2))))))) / (-9.81f * Mathf.Abs(heading.x)));
        //angleY = Mathf.Atan(((Mathf.Pow(100, 2)) - Mathf.Sqrt(Mathf.Pow(100, 4) - -9.81f * (-9.81f * (Mathf.Pow(Mathf.Abs(heading.x), 2) + (2 * Mathf.Abs(heading.y) * (Mathf.Pow(100, 2))))))) / (-9.81f * Mathf.Abs(heading.x)));
        //Debug.Log(angle);
        //Debug.Log(angleX);
        //Debug.Log(angleY);

        //Only works in x direction. Doesnt accomidate for y direction of aiming.
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        rigidbodyComponent.AddForce(dir * 1350.0f);
    }

}
