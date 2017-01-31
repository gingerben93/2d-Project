using UnityEngine;
using UnityEditor;
using System.Collections;

public class MoveTowardPlayer : MonoBehaviour {

    private Transform enemy;
    private Transform player;

    //
    private Rigidbody2D enemyRigBody;
    
    //
    private float maxSpeed;

    //
    private Vector3 heading;
    private float distance;
    private Vector3 StartDirection;
    private float distanceToPlayer;

    //for enemy random movement
    private int pickDirection;
    private bool randomDirection;
    private float enemySpeed;

    private float noMovementThreshold;
    private int noMovementFrames;
    private Vector3[] previousLocations;

    private float scriptDelayAmount;
    private float scriptDelayLastTime;

    // Use this for initialization
    void Start ()
    {
        //enemy = transform.GetComponentInParent<Transform>();
        enemy = transform.parent.transform;
        player = GameObject.Find("Hero").transform;

        //
        enemyRigBody = enemy.GetComponent<Rigidbody2D>();

        //
        maxSpeed = 2f;

        //for enemy direction
        randomDirection = false;
        noMovementThreshold = 0.0001f;
        noMovementFrames = 3;
        previousLocations = new Vector3[noMovementFrames];

        scriptDelayAmount = .5f;
        scriptDelayLastTime = 0;
}

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // make function for deciding when player can jump. 
        /*
        set a min x and max x distance (probably y's too)
        after min x and max x are found, try to jump at those x,y coordinates. (should be direction related also)
        (don't always have to jump, just add that probablity in)
        If it works, then check  for new x max and x min.
        if it fails (maybe a number of times) then stop jumping around that x (and y).

        append those the x,y jumps when the min or max changes. 
        reset if both min and max become new.

        */
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = transform.position;

        distanceToPlayer = Vector3.Distance(enemy.position, player.position);

        if (distanceToPlayer <= 10)
        {
            // set for picking direction after player leaves range
            randomDirection = false;

            if (distanceToPlayer <= 5)
            {
                enemyRigBody.velocity = new Vector2(enemyRigBody.velocity.x * .95f, enemyRigBody.velocity.y);
                scriptDelayLastTime = Time.time + scriptDelayAmount;
            }
            else
            {
                //for jumping if chasing player and got stuck
                if (scriptDelayLastTime < Time.time)
                {
                    for (int i = 0; i < previousLocations.Length - 1; i++)
                    {
                        if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) <= noMovementThreshold)
                        {
                            enemyRigBody.AddForce(new Vector2(0f, 500));
                            scriptDelayLastTime = Time.time + scriptDelayAmount;
                        }
                    }
                }

                heading = player.position - enemy.position;
                distance = heading.magnitude;
                StartDirection = heading / distance;
                enemyRigBody.velocity = new Vector2(maxSpeed * StartDirection.x, enemyRigBody.velocity.y);
            }
        }
        else
        {
            if (scriptDelayLastTime < Time.time)
            {
                //only pick start variables once
                if (!randomDirection)
                {
                    randomDirection = true;
                    pickDirection = Random.Range(0, 100);

                    if (pickDirection < 50)
                    {
                        enemySpeed = maxSpeed;
                    }
                    else
                    {
                        enemySpeed = -maxSpeed;
                    }
                }

                enemyRigBody.velocity = new Vector2(enemySpeed, enemyRigBody.velocity.y);

                //check for hitting wall
                for (int i = 0; i < previousLocations.Length - 1; i++)
                {
                    pickDirection = Random.Range(0, 100);


                    if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) <= noMovementThreshold)
                    {
                        if (pickDirection < 49)
                        {
                            enemySpeed *= -1;
                            scriptDelayLastTime = Time.time + scriptDelayAmount;
                        }

                        else
                        {
                            enemyRigBody.AddForce(new Vector2(0f, 500));
                            scriptDelayLastTime = Time.time + scriptDelayAmount;
                        }
                    }
                }
            }
            else
            {
                //waiting
            }
        }
	}
}
