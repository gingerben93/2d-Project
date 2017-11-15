using UnityEngine;
using System.Collections;

public class GrapplingHookTip : MonoBehaviour {

    DrawGrapHook drawGrapHook;

    void Start()
    {
        drawGrapHook = transform.parent.GetComponentInChildren<DrawGrapHook>();

        //get the rb2d that joint is connected to
        GetComponent<DistanceJoint2D>().connectedBody = PlayerController.PlayerControllerSingle.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D tip)
    {
        if (tip.gameObject.GetComponent<EdgeCollider2D>())
        {

        }

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<DistanceJoint2D>().distance = Vector2.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position);
        GetComponent<DistanceJoint2D>().enabled = true;

        drawGrapHook.HasTipCollided = true;

        if (tip.tag == "Enemy" && drawGrapHook.hitEnemy == false)
        {
            drawGrapHook.InstanceID = tip.GetInstanceID();
            drawGrapHook.hitEnemy = true;
            drawGrapHook.enemyTransform = tip.transform;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && drawGrapHook.InstanceID == coll.GetInstanceID())
        {
            drawGrapHook.hitEnemy = false;
            drawGrapHook.InstanceID = 0;
        }
    }
}
