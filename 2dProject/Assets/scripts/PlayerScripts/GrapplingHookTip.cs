using UnityEngine;
using System.Collections;

public class GrapplingHookTip : MonoBehaviour {

    DrawGrapHook HasTipCollided;

    void Start()
    {
        HasTipCollided = FindObjectOfType<DrawGrapHook>();
    }

    public void OnCollisionEnter2D(Collision2D tip)
    //public void OnTriggerEnter2D(Collider2D tip)
    {
        Debug.Log("tip collide");
        HasTipCollided.HasTipCollided = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
    }
}
