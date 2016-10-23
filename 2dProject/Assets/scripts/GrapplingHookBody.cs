using UnityEngine;
using System.Collections;

public class GrapplingHookBody : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D tip)
    {
        DrawGrapHook hasBodyCollided = FindObjectOfType<DrawGrapHook>();
        hasBodyCollided.hasBodyCollided = true;
    }
    public void OnTriggerStay2D(Collider2D tip)
    {
        DrawGrapHook hasBodyCollided = FindObjectOfType<DrawGrapHook>();
        hasBodyCollided.hasBodyCollided = true;
    }
}
