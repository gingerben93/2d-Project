﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandsFistPlayer : MonoBehaviour
{

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

    //gets set in GodHands script
    public GodHands parent;

    //speed of fist
    float fistSpeed = 500;
    float step;

    void Start()
    {
        //turn attack off
        parent.GodhandsCanAttack = false;
    }

    void FixedUpdate()
    {
        step = fistSpeed * Time.deltaTime;
        if (pullAttackOn)
        {
            userTransform.position = Vector3.MoveTowards(userTransform.position, newTargetLocation, step);

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
            userTransform.position = Vector3.MoveTowards(userTransform.position, PlayerController.PlayerControllerSingle.transform.position, step);

            if (returnWithPlayer)
            {
                targetTransform.position = Vector3.MoveTowards(userTransform.position, PlayerController.PlayerControllerSingle.transform.position, step);
            }
            if (Vector3.Distance(userTransform.position, PlayerController.PlayerControllerSingle.transform.position) <= .5f)
            {
                returnFist = false;

                //if hit target, knock it up
                if (returnWithPlayer)
                {
                    targetRB.AddForce(new Vector2(0, 500));
                    returnWithPlayer = false;
                }

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if enemey get the RB and move it
        if (other.tag == "Enemy")
        {
            targetRB = other.GetComponent<Rigidbody2D>();
            hitObject = true;
            targetTransform = other.transform;
        }
        
    }

    void OnDestroy()
    {
        //turn attack back on so you can fire godhands again
        parent.GodhandsCanAttack = true;
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    Debug.Log("leaving player");
    //    hitObject = false;
    //}
}
