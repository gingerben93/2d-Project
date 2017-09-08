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
    int Direction;

    // Use this for initialization
    void Start ()
    {
        try
        {
            //get random edge collider
            ColliderHolder = GameObject.Find("ColliderHolder");
            currentEdgeSet = Random.Range(0, ColliderHolder.transform.childCount);
            edge = ColliderHolder.transform.GetChild(currentEdgeSet).GetComponent<EdgeCollider2D>();

            //set information
            currentEdgePoint = 0;
            gameObject.transform.position = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);
            //OnDrawGizmos();
            Direction = Random.Range(0, 2);
 
        }
        catch
        {
            Debug.Log("initialized to early");
        }
    }

    void Update()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        CurrentVector = new Vector2(edge.points[currentEdgePoint].x, edge.points[currentEdgePoint].y);

        //gets current perp vector with current and next point
        knownVec = CurrentVector - new Vector2(edge.points[(currentEdgePoint + 1) % edge.pointCount].x, edge.points[(currentEdgePoint + 1) % edge.pointCount].y);
        perpVec = CurrentVector + new Vector2(knownVec.y, -knownVec.x) * 1;

        //for moving the object
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, perpVec, .25f);

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

    //void OnDrawGizmos()
    //{
    //    float slope;
        
    //    for (int y = 0; y < edge.edgeCount - 1; y++)
    //    {
    //        knownVec = new Vector2(edge.points[y].x, edge.points[y].y) - new Vector2(edge.points[(y + 1) % edge.pointCount].x, edge.points[(y + 1) % edge.pointCount].y);
    //        perpVec = new Vector2(knownVec.y, -knownVec.x);
    //        //perpVec.Normalize();

    //        slope = -1 / (edge.points[(y + 1) % edge.pointCount].y - edge.points[y].y / edge.points[(y + 1) % edge.pointCount].x - edge.points[y].x);

    //        Gizmos.color = Color.black;
    //        Gizmos.DrawCube(new Vector2(edge.points[y].x, edge.points[y].y) + perpVec * 1, Vector2.one/5);
    //    }
    //}
}
