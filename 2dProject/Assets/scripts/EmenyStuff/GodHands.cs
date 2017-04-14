using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHands : MonoBehaviour {

    // Update is called once per frame
    bool pullAttackOn = false;
    bool returnFist = false;
    bool returnWithPlayer = false;

    bool hitObject = false;


    private Transform targetTransform;
    private Transform userTransform;

    Rigidbody2D targetRB;

    public Vector3 newTargetLocation;


    void Start()
    {
        userTransform = transform.parent.transform;
    }

	void Update ()
    {
        if (pullAttackOn)
        {
            transform.position = Vector3.MoveTowards(transform.position, newTargetLocation, .5f);

            if (hitObject)
            {
                pullAttackOn = false;
                returnWithPlayer = true;
                returnFist = true;
            }
            else if (Vector3.Distance(transform.position, newTargetLocation) <= .1f)
            {
                pullAttackOn = false;
                returnFist = true;
            }
        }
        else if (returnFist)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.transform.position, 1);

            if (returnWithPlayer)
            {
                targetTransform.position = Vector3.MoveTowards(transform.position, transform.parent.transform.position, .5f);
            }
            if (Vector3.Distance(transform.position, transform.parent.transform.position) == 0)
            {
                returnFist = false;
                
                if (returnWithPlayer)
                {
                    targetRB.AddForce(new Vector2(0, 500));
                    returnWithPlayer = false;
                }
            }
        }
    }

    public void Attack(Vector3 targetLocation)
    {
        pullAttackOn = true;
        newTargetLocation = targetLocation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {
            targetRB = other.GetComponent<Rigidbody2D>();
            hitObject = true;
            targetTransform = other.transform;
            Debug.Log("hit player");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("leaving player");
        hitObject = false;
    }

}
