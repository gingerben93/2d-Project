using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPathfinding : MonoBehaviour {

    float speed = .1f;
    int distanceDetection = 4;
    //Vector3 raycastOffSet;
    float rayTurnLeft;
    float rayTurnRight;
    float angleOffSetLeft, angleOffSetRight = 0f;

    bool frontRayHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        Pathfinding();
        
    }

    void Pathfinding()
    {
        
        //raycastOffSet = Vector3.zero;

        Vector3 left = transform.position + transform.up;
        Vector3 right = transform.position - transform.up;

        Vector3 leftSideCheck = transform.position + transform.up;
        Vector3 rightSideCheck = transform.position - transform.up;

        RaycastHit2D hitLeft = Physics2D.Raycast(left, transform.right, distanceDetection);
        RaycastHit2D hitRight = Physics2D.Raycast(right, transform.right, distanceDetection);

        Debug.DrawRay(left, transform.right * distanceDetection, Color.blue);
        Debug.DrawRay(right, transform.right * distanceDetection, Color.blue);

        frontRayHit = false;

        if (hitLeft)
        {
            //raycastOffSet -= Vector3.forward;
            if (rayTurnLeft <= 1)
            {
                rayTurnLeft += .1f;
                angleOffSetLeft -= 9f;
            }
            else
            {
                Debug.Log("left side full");
                rayTurnLeft = 1;
            }

            frontRayHit = true;
        }
        else if (hitRight)
        {
            //raycastOffSet += Vector3.forward;
            if (rayTurnRight <= 1)
            {
                rayTurnRight += .1f;
                angleOffSetRight += 9f;
            }
            else
            {
                Debug.Log("right side full");
                rayTurnRight = 1;
            }

            frontRayHit = true;
        }

        //Debug.DrawRay(leftSideCheck, new Vector3(-transform.right.y, transform.right.x, 0 )* distanceDetection, Color.red);
        //Debug.DrawRay(rightSideCheck, new Vector3(transform.right.y, -transform.right.x, 0) * distanceDetection, Color.red);

        Vector3 leftSide = (new Vector3(-transform.right.y * rayTurnLeft + transform.right.x * (1 - rayTurnLeft), (transform.right.x * rayTurnLeft + transform.right.y * (1 - rayTurnLeft)), 0));
        Vector3 rightSide = (new Vector3(transform.right.y * rayTurnRight + transform.right.x * (1 - rayTurnRight), (-transform.right.x * rayTurnRight + transform.right.y * (1 - rayTurnRight)), 0));

        Debug.DrawRay(leftSideCheck, leftSide * distanceDetection, Color.red);
        Debug.DrawRay(rightSideCheck, rightSide * distanceDetection, Color.red);

        RaycastHit2D leftSideHit = Physics2D.Raycast(leftSideCheck, leftSide, distanceDetection);
        RaycastHit2D rightSideHit = Physics2D.Raycast(rightSideCheck, rightSide, distanceDetection);


        if (leftSideHit)
        {
            
        }
        else
        {
            angleOffSetLeft = 0;
            rayTurnLeft = 0;
        }

        if (rightSideHit)
        {
            
        }
        else
        {
            angleOffSetRight = 0;
            rayTurnRight = 0;
        }

        Vector3 delta = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + angleOffSetLeft + angleOffSetRight);
        transform.rotation = rotation;

        if (!frontRayHit)
        {
            //transform.position += transform.right * speed;
        }

        transform.position += transform.right * speed;

        //Vector3 delta = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        //float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.Euler(0, 0, angle + angleOffSet);
        //transform.rotation = rotation;

        //if (raycastOffSet != Vector3.zero)
        //{
        //    transform.Rotate(raycastOffSet * 3f);
        //}
        //else
        //{
        //    Vector3 delta = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        //    float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        //    Quaternion rotation = Quaternion.Euler(0, 0, angle + angleOffSet);
        //    transform.rotation = rotation;
        //}
    }
}
