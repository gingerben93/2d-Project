using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandsFist : MonoBehaviour {

    public bool pullAttackOn = false;
    public bool returnFist = false;
    public bool returnWithPlayer = false;
    public bool returnFistPlayer = false;
    public bool returnWithEnemy = false;

    public bool hitObject = false;

    //for user attack, not same as boss
    public bool userAttackOn = false;
    public Vector3 gotoPosition;

    public Transform targetTransform;
    public Transform userTransform;

    public Rigidbody2D targetRB;

    public Vector3 newTargetLocation;

    public Transform fistPrefab;

    //speed of fist
    //float fistSpeed = 100;
    //float step;

    void FixedUpdate()
    {
        //step = fistSpeed * Time.deltaTime;
        if (pullAttackOn)
        {
            userTransform.position = Vector3.MoveTowards(userTransform.position, newTargetLocation, .5f);

            if (hitObject)
            {
                pullAttackOn = false;
                returnWithPlayer = true;
                returnFist = true;
            }
            else if (Vector3.Distance(userTransform.position, newTargetLocation) <= .1f)
            {
                pullAttackOn = false;
                returnFist = true;
            }
        }
        else if (returnFist)
        {
            userTransform.position = Vector3.MoveTowards(userTransform.position, transform.parent.position, 1f);

            if (returnWithPlayer)
            {
                targetTransform.position = Vector3.MoveTowards(userTransform.position, transform.parent.position, 1f);
            }
            if (Vector3.Distance(userTransform.position, transform.parent.position) == 0)
            {
                returnFist = false;

                if (returnWithPlayer)
                {
                    targetRB.AddForce(new Vector2(0, 1000));
                    returnWithPlayer = false;
                }

                Destroy(gameObject);
                //if hit target, knock it up
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Pulls PLayer and make it so player doesnt interact with the shield
            Shield.ShieldSingle.notGrabbed = false;
            targetRB = other.GetComponent<Rigidbody2D>();
            hitObject = true;
            targetTransform = other.transform;
            
        }

        if (other.tag == "BossRoomItem")
        {
            targetRB = other.GetComponent<Rigidbody2D>();
            hitObject = true;
            targetTransform = other.transform;
        }

        if (other.tag == "Bullet")
        {
            Debug.Log("HIT by Bullet fist");
            Debug.Log(GetComponent<Collider2D>().name);
        }
    }

}
