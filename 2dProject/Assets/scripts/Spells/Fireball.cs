using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{

    private Vector3 StartLocation, EndLocation;

    //private Vector3 mousePos;
    private Vector3 heading;
    private float distance;
    private Vector3 direction;

    //arc curve variables
    float angle;
    float angleX;
    float angleY;

    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void FixedUpdate()
    {
        //Debug.Log("vel = " + rigidbodyComponent.velocity);
    }

    //start method
    public void SetStartData(Vector3 Start, Vector3 End)
    {
        StartLocation = Start;
        EndLocation = End;

        //get fireball rb
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        //set start locations of object
        transform.position = Start;
        //get firball velocity
        Shoot();
    }

    void Shoot()
    {

        float g, maxHeight, time, displacementY;
        Vector3 velocityY, velocityXZ, displacementXZ;
        g = -9.81f;

        maxHeight = EndLocation.y - StartLocation.y;

        if(0 < maxHeight && maxHeight < 3)
        {
            maxHeight = 3;
        }

        displacementY = EndLocation.y - StartLocation.y;
        displacementXZ = new Vector3(EndLocation.x - StartLocation.x, 0, EndLocation.z - StartLocation.z);
        time = Mathf.Sqrt(-2 * maxHeight / g) + Mathf.Sqrt(2 * (displacementY - maxHeight) / g);

        //for above player
        if (displacementY <= 0)
        {
            time = Mathf.Sqrt(2 * displacementY / g);
            velocityY = Vector3.zero;
            velocityXZ = displacementXZ / time;
        }
        //for below player
        else
        {
            time = Mathf.Sqrt(-2 * maxHeight / g) + Mathf.Sqrt(2 * (displacementY - maxHeight) / g);
            velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxHeight);
            velocityXZ = displacementXZ / time;
        }

        //set velocity x y in rb
        rigidbodyComponent.velocity = (velocityXZ + velocityY * -Mathf.Sign(g));
    }
}
