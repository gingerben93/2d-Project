using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    private Vector3 StartLocation, EndLocation;
    private Rigidbody2D rigidbodyComponent;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void FixedUpdate()
    {
        CalculateRotation();
    }

    void CalculateRotation()
    {
        //faces object forward
        float angle = Mathf.Atan2(rigidbodyComponent.velocity.y, -rigidbodyComponent.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
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
        CalculateSpeed();
        CalculateRotation();
    }

    void CalculateSpeed()
    {

        float g, maxHeight, time, displacementY;
        Vector3 velocityY, velocityXZ, displacementXZ;
        g = -9.81f;

        maxHeight = EndLocation.y - StartLocation.y;
        displacementY = EndLocation.y - StartLocation.y;

        if (0 < maxHeight && maxHeight < 3)
        {
            maxHeight = 3;
        }

        //max height must be positive // can we find a fix for this?
        if (displacementY <= 0)
        {
            if (displacementY == 0)
            {
                maxHeight = 3f;
                displacementY = 3f;
            }
            else
            {
                maxHeight = 1;
            }
        }

        displacementXZ = new Vector3(EndLocation.x - StartLocation.x, 0, EndLocation.z - StartLocation.z);
        time = Mathf.Sqrt(-2 * maxHeight / g) + Mathf.Sqrt(2 * (displacementY - maxHeight) / g);
        velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxHeight);
        velocityXZ = displacementXZ / time;

        ////for below player
        //if (displacementY <= 0)
        //{
        //    time = Mathf.Sqrt(2 * displacementY / g);
        //    velocityY = Vector3.zero;
        //    velocityXZ = displacementXZ / time;
        //}
        ////for above player
        //else
        //{
        //    velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxHeight);
        //    velocityXZ = displacementXZ / time;
        //}

        //set velocity x y in rb
        rigidbodyComponent.velocity = (velocityXZ + velocityY * -Mathf.Sign(g));
    }
}
