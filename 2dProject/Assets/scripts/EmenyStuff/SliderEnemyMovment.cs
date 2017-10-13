using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEnemyMovment : MonoBehaviour {

    private EdgeCollider2D edge;
    int currentEdgePoint = 0;
    GameObject ColliderHolder;
    int currentEdgeSet;
    Vector2 CurrentVector;
    Vector2 knownVec;
    Vector2 perpVec;
    Quaternion targetRotation;

    //random variables
    int Direction;
    float speed;

    //for attack
    Transform PlayerTransform;
    public bool attacking = false;
    SliderEnemyAttack Attack;

    //attack cooldown when destoryed
    float coolDown = 2;
    float coolDownTimer = 2;



    // Use this for initialization
    void Start ()
    {
        //for attack
        PlayerTransform = PlayerController.PlayerControllerSingle.transform;
        Attack = GetComponentInChildren<SliderEnemyAttack>();



        try
        {
            //get random edge collider
            ColliderHolder = GameObject.Find("ColliderHolder");
            currentEdgeSet = Random.Range(0, ColliderHolder.transform.childCount);
            edge = ColliderHolder.transform.GetChild(currentEdgeSet).GetComponent<EdgeCollider2D>();

            //set information
            currentEdgePoint = 0;
            gameObject.transform.position = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);
            
            //random variables
            Direction = Random.Range(0, 2);
            speed = Random.Range(20, 40) / 100f;
 
        }
        catch
        {
            Debug.Log("initialized to early");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerTransform.position) <= 1000f && !attacking)
        {
            if (coolDownTimer == 0)
            {
                Debug.Log("move again");
                coolDownTimer = coolDown;
                attacking = true;
                Attack.StartAttack();

                //get new random direction when moving again
                Direction = Random.Range(0, 2);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (attacking)
        {
            //now gets set in attack script
            //attacking = Attack.attacking;
        }
        else
        {
            //cool down for attack
            if (coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }
            else
            {
                coolDownTimer = 0;
            }

            try
            {
                CurrentVector = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);

                //gets current perp vector with current and next point
                knownVec = CurrentVector - new Vector2(edge.points[(currentEdgePoint + 1) % edge.pointCount].x, edge.points[(currentEdgePoint + 1) % edge.pointCount].y);
                perpVec = CurrentVector + new Vector2(knownVec.y, -knownVec.x) * 1;

                //for moving the object
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, perpVec, speed);

                //for rotating the object
                targetRotation = Quaternion.LookRotation(Vector3.forward, perpVec - CurrentVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .25f);

                //gets next point to move to
                if (Vector3.Distance(gameObject.transform.position, perpVec) <= .1f)
                {
                    if (Direction == 0)
                    {
                        currentEdgePoint = (currentEdgePoint + 1) % (edge.edgeCount - 1);
                    }
                    else
                    {
                        currentEdgePoint = ((edge.edgeCount - 1) + currentEdgePoint - 1) % (edge.edgeCount - 1);
                    }
                }
            }
            catch
            {
                try
                {
                    //get random edge collider
                    ColliderHolder = GameObject.Find("ColliderHolder");
                    currentEdgeSet = Random.Range(0, ColliderHolder.transform.childCount);
                    edge = ColliderHolder.transform.GetChild(currentEdgeSet).GetComponent<EdgeCollider2D>();

                    //set information
                    currentEdgePoint = Random.Range(0, edge.edgeCount);
                    gameObject.transform.position = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);
                    Direction = Random.Range(0, 2);
                }
                catch
                {
                    //needs time to spawn
                }
            }
        }
    }
}
