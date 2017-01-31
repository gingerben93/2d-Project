using UnityEngine;
using System.Collections;

public class GrapplingHookTip : MonoBehaviour {

    DrawGrapHook HasTipCollided;

    void Start()
    {
        HasTipCollided = FindObjectOfType<DrawGrapHook>();
    }

    public void OnTriggerEnter2D(Collider2D tip)
    {
        HasTipCollided.HasTipCollided = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
