using UnityEngine;
using System.Collections;

public class GrapplingHookTip : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D tip)
    {
        DrawGrapHook HasTipCollided = FindObjectOfType<DrawGrapHook>();
        HasTipCollided.HasTipCollided = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}
