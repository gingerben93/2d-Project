using UnityEngine;
using System.Collections;

public class GrapplingHookTip : MonoBehaviour {

    DrawGrapHook HasTipCollided;

    void Start()
    {
        HasTipCollided = FindObjectOfType<DrawGrapHook>();
        
        //get the rb2d that joint is connected to
        GetComponent<DistanceJoint2D>().connectedBody = PlayerController.PlayerControllerSingle.GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D tip)
    {
        HasTipCollided.HasTipCollided = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<DistanceJoint2D>().distance = Vector2.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position);
        GetComponent<DistanceJoint2D>().enabled = true;
        //gameObject.GetComponent<Rigidbody2D>().simulated = false;
    }
}
