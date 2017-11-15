using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookBody2 : MonoBehaviour {

    DrawGrapHook drawGrapHook;

    void Start ()
    {
        drawGrapHook = transform.parent.GetComponentInChildren<DrawGrapHook>();
	}
	
	void Update ()
    {
		
	}

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy" && drawGrapHook.hitEnemy == false)
        {
            drawGrapHook.InstanceID = coll.GetInstanceID();
            drawGrapHook.hitEnemy = true;
            drawGrapHook.enemyTransform = coll.transform;
        }
    }
}
